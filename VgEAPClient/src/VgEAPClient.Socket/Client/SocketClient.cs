// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Net;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using SuperSocket.Client;
using SuperSocket.ProtoBase;
using VgAutoDrill.Infrastructure;
using VgEAPClient.Socket.Models;

namespace VgEAPClient.Socket.Client;

public class SocketClient : ISokcetClient
{
    public event OnHostRequestEvent? OnHostRequest;
    public event OnHostConnectEvent? OnHostConnect;
    public event OnHostCloseEvent? OnHostClose;
    public event OnHostConnectErrorEvent? OnHostConnectError;

    public readonly ILogger<SocketClient> _logger;
    public readonly IEasyClient<EapMessage> _easyClient;
    public readonly IPackageEncoder<EapMessage> _packageEncoder;
    public readonly IAsyncTaskWaiter<EapMessage> _taskWaiter;
    public bool _isClosing = false;
    public CancellationToken _cancellationToken;

    public SocketClient(IOptions<EapServerOptions> options,
        IPackageEncoder<EapMessage> packageEncoder,
        IPackageDecoder<EapMessage> packageDecoder,
        IPipelineFilter<EapMessage> pipelineFilter,
        IAsyncTaskWaiter<EapMessage> taskWaiter,
        ILoggerFactory loggerFactory)
    {
        Options = options.Value;
        _logger = loggerFactory.CreateLogger<SocketClient>();
        _easyClient = new EasyClient<EapMessage>(pipelineFilter);

        _easyClient.PackageHandler += OnPackageReceived;
        _easyClient.Closed += OnEasyClientClosed;

        pipelineFilter.Decoder = packageDecoder;
        _packageEncoder = packageEncoder;
        _taskWaiter = taskWaiter;
    }

    public bool IsConnected { get; private set; }

    public EapServerOptions Options { get; private set; }

    public Task Connect(CancellationToken cancellationToken = default)
    {
        if (IsConnected) return Task.CompletedTask;
        _cancellationToken = cancellationToken;

        return Task.Run(async () =>
        {
            _logger.LogInformation($"EapClient is connecting to {Options.Host}:{Options.Port}...");
            _isClosing = false;
            await DoConnect(_cancellationToken);
        });
    }

    public virtual async Task<EapMessage> SendMessage(EapMessage request, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation($"EapClient is Sending Message: {request}");
        if (string.IsNullOrWhiteSpace(request.Header.MessageName) || !EapBodyModelMapping.ModelTypes.ContainsKey(request.Header.MessageName))
        {
            _logger.LogError("request.Header.MessageName is empty or not valid!");
            throw new ArgumentException(nameof(request.Header.MessageName));
        }

        if (string.IsNullOrWhiteSpace(request.Header.TransactionId))
        {
            _logger.LogError("request.Header.TransactionId is empty!");
            throw new ArgumentException(nameof(request.Header.TransactionId));
        }

        var sentTransactionId = request.Header.TransactionId;

        try
        {
            var awaitable = new AsyncTaskCompletionSource<EapMessage>();

            if (!_taskWaiter.TryAdd(sentTransactionId, awaitable))
            {
                throw new InvalidOperationException();
            }

            await _easyClient.SendAsync(_packageEncoder, request).ConfigureAwait(false);

            using (cancellationToken.Register(() => awaitable.TrySetCanceled()))
            {
                return await awaitable.Task.ConfigureAwait(false);
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.Message);
            throw;
        }
        finally
        {
            _taskWaiter.TryRemove(sentTransactionId);
        }
    }

    public virtual async Task PostMessage(EapMessage request, CancellationToken cancellationToken = default)
    {
        await _easyClient.SendAsync(_packageEncoder, request).ConfigureAwait(false);
    }

    public async Task<EapMessage> PostByet(byte[] bytes, string TransactionId, CancellationToken cancellationToken = default)
    {
        EapMessage eap = new EapMessage();
        try
        {
            var tokenSource = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);
            tokenSource.CancelAfter(TimeSpan.FromSeconds(30));
            cancellationToken = tokenSource.Token;
            _logger.LogInformation($@"PostByetWait({TransactionId})");
            eap = await _taskWaiter.ExecuteTaskAsWaiter(TransactionId,
            PostByet(bytes),
            cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogInformation($@"PostByetWaitError({TransactionId})");
            _logger.LogError(ex, $@"PostByetWait - {ex.Message}");
            _taskWaiter.TryRemove(TransactionId);
            eap = new EapMessage
            {
                Return = new EapMessage.EapReturn
                {
                    ReturnCode = "504",
                    ReturnMessage = ex.Message
                }
            };
        }
        return eap;
    }

    public async Task PostByet(byte[] bytes)
    {
        await _easyClient.SendAsync(bytes).ConfigureAwait(false);
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    private async Task DoConnect(CancellationToken cancellationToken = default)
    {
        IsConnected = await _easyClient.ConnectAsync(new IPEndPoint(IPAddress.Parse(Options.Host), Options.Port), cancellationToken);
        if (IsConnected)
        {
            _logger.LogInformation($"EapClient connect to {Options.Host}:{Options.Port} successfully!");
            OnHostConnect?.Invoke(Options.Host, Options.Port);
            _easyClient.StartReceive();
        }
        else
        {
            _logger.LogError($"EapClient cannot connect to {Options.Host}:{Options.Port}, please make sure EAP server is running or check EAP Client EapServerOptions!");
            OnHostConnectError?.Invoke(Options.Host, Options.Port);
            Thread.Sleep(1000);
            await DoConnect();
        }
    }

    private async void OnEasyClientClosed(object? sender, EventArgs e)
    {
        OnHostClose?.Invoke(Options.Host, Options.Port);
        if (Options.AutoReconnect && !_isClosing)
        {
            await DoConnect(_cancellationToken);
        }
    }

    public virtual async ValueTask OnPackageReceived(EasyClient<EapMessage> sender, EapMessage package)
    {
        var sentTransactionId = package.Header.TransactionId;
        _taskWaiter.TrySetResult(sentTransactionId, package);
        if (!package.Header.MessageName.EndsWith("Reply") && OnHostRequest != null)
        {
            await OnHostRequest.Invoke(package);
        }
    }

    public Task OnHostRequestInvoke(EapMessage package)
    {
        OnHostRequest?.Invoke(package);
        return Task.CompletedTask;
    }

    protected virtual async void Dispose(bool disposing)
    {
        if (_isClosing)
            return;

        if (disposing)
        {
            _isClosing = true;
            await _easyClient.CloseAsync();
            _easyClient.PackageHandler -= OnPackageReceived;
            _easyClient.Closed -= OnEasyClientClosed;

            if (OnHostRequest is null)
            {
                return;
            }

            foreach (var @delegate in OnHostRequest.GetInvocationList())
            {
                Delegate.Remove(OnHostRequest, @delegate);
            }

            OnHostRequest = null;
        }
    }


}
