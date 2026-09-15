using System.Buffers;
using System.Buffers.Binary;
using System.IO.Pipes;
using System.Security.Principal;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using VgAutoDrill.Fundation.Iot;
using VgAutoDrill.Fundation.Iot.Models;
using VgAutoDrill.Fundation.Utils;

namespace VgAutoDrill.Fundation.Pipe;

public class PipeClient : IPipeClient
{
    private readonly PipeClientOptions _pipeConnectionOptions;
    private readonly ILogger<PipeClient> _logger;
    private NamedPipeClientStream? _pipeClient = null;
    private readonly IPacketDecoder _packetDecoder;
    private readonly IPacketEncoder _packetEncoder;
    private readonly PeriodicTimer _periodicTimer;
    private TaskCompletionSource<DeviceServiceInvokeResponse>? _taskCompletionSource;

    public PipeClient(IOptions<PipeClientOptions> options,
        IPacketDecoder packetDecoder,
        IPacketEncoder packetEncoder,
        ILoggerFactory loggerFactory)
    {
        _pipeConnectionOptions = options.Value;
        _logger = loggerFactory.CreateLogger<PipeClient>();
        _periodicTimer = new PeriodicTimer(TimeSpan.FromMilliseconds(500));
        _packetDecoder = packetDecoder;
        _packetEncoder = packetEncoder;
    }

    public bool IsConnected => _pipeClient != null && _pipeClient.IsConnected;

    public async Task ConnectAsync(CancellationToken cancellationToken = default)
    {
        ArgumentException.ThrowIfNullOrEmpty(_pipeConnectionOptions.ServerName);
        ArgumentException.ThrowIfNullOrEmpty(_pipeConnectionOptions.PipeName);
        if (!_pipeConnectionOptions.Enabled)
            return;

        _pipeClient = new NamedPipeClientStream(_pipeConnectionOptions.ServerName,
            _pipeConnectionOptions.PipeName,
            PipeDirection.InOut,
            PipeOptions.WriteThrough | PipeOptions.Asynchronous,
            TokenImpersonationLevel.Impersonation);

        _logger.LogInformation("Connecting to server...");
        await _pipeClient.ConnectAsync(cancellationToken).ConfigureAwait(false);

        _ = Task.Factory.StartNew(async () =>
        {
            using (_periodicTimer)
            {
                while (!cancellationToken.IsCancellationRequested
                    && await _periodicTimer.WaitForNextTickAsync())
                {
                    await ReceiveMessage(cancellationToken);
                }
            }
        }, TaskCreationOptions.LongRunning);
    }

    public async Task<DeviceServiceInvokeResponse> Request(DeviceServiceInvokeRequest deviceServiceInvokeRequest, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(_pipeClient);
        ArgumentNullException.ThrowIfNull(deviceServiceInvokeRequest);
        ThrowHelper.ThrowArgumentNullOrWhiteSpaceException(deviceServiceInvokeRequest.ServiceId);
        ThrowHelper.ThrowArgumentNullOrWhiteSpaceException(deviceServiceInvokeRequest.DeviceId);

        _taskCompletionSource = new TaskCompletionSource<DeviceServiceInvokeResponse>();
        SendContent(deviceServiceInvokeRequest);
        cancellationToken.Register(() => _taskCompletionSource.TrySetCanceled());
        return await _taskCompletionSource.Task;
    }

    public Task Post(DeviceServiceInvokeRequest deviceServiceInvokeRequest)
    {
        ArgumentNullException.ThrowIfNull(_pipeClient);
        ArgumentNullException.ThrowIfNull(deviceServiceInvokeRequest);
        ThrowHelper.ThrowArgumentNullOrWhiteSpaceException(deviceServiceInvokeRequest.ServiceId);
        ThrowHelper.ThrowArgumentNullOrWhiteSpaceException(deviceServiceInvokeRequest.DeviceId);

        deviceServiceInvokeRequest.InvocationKind = ServiceInvocationKind.Post;
        SendContent(deviceServiceInvokeRequest);
        return Task.CompletedTask;
    }

    public async Task DisconnectAsync()
    {
        if (_pipeClient != null)
        {
            _pipeClient.Close();
            _periodicTimer.Dispose();
            await _pipeClient.DisposeAsync().ConfigureAwait(false);
        }
    }

    private async Task ReceiveMessage(CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(_pipeClient);

        if (!cancellationToken.IsCancellationRequested
             && _taskCompletionSource != null)
        {
            byte[] buffer = ArrayPool<byte>.Shared.Rent(_pipeConnectionOptions.MinBufferSize);
            int read = await _pipeClient.ReadAsync(buffer, 0, buffer.Length, cancellationToken);

            try
            {
                if (read > 4)
                {
                    var bodysize = BinaryPrimitives.ReadInt32BigEndian(new ReadOnlySpan<byte>(buffer, 0, 4));
                    if (bodysize > 0 && read >= bodysize - 4)
                    {
                        var deviceServiceInvokeResponse = _packetDecoder.Decode<DeviceServiceInvokeResponse>(new ReadOnlySpan<byte>(buffer, 4, bodysize)) ?? new DeviceServiceInvokeResponse
                        {
                            Code = ErrorCodes.Sys.FAIL,
                            Message = $"DeviceServiceInvokeResponse can't be deserialized!"
                        };
                        _logger.LogInformation($"ReceivedMessage={deviceServiceInvokeResponse.ToJson()}");
                        _taskCompletionSource.TrySetResult(deviceServiceInvokeResponse);
                    }
                }
            }
            finally
            {
                ArrayPool<byte>.Shared.Return(buffer);
            }
        }
    }

    private void SendContent(DeviceServiceInvokeRequest request)
    {
        ArgumentNullException.ThrowIfNull(_pipeClient);
        byte[] bytes = _packetEncoder.Encode(request);
        byte[] buffer = ArrayPool<byte>.Shared.Rent(4 + bytes.Length);
        BinaryPrimitives.WriteInt32BigEndian(buffer, bytes.Length);
        Array.Copy(bytes, 0, buffer, 4, bytes.Length);
        _pipeClient.Write(buffer);
        _pipeClient.Flush();
        ArrayPool<byte>.Shared.Return(buffer);
        _logger.LogInformation($"SentContent={request.ToJson()}");
    }
}
