using System.Buffers;
using System.Buffers.Binary;
using System.IO.Pipes;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using VgAutoDrill.Fundation.Iot;
using VgAutoDrill.Fundation.Iot.Models;
using VgAutoDrill.Fundation.Utils;

namespace VgAutoDrill.Fundation.Pipe;

internal class PipeServer : BackgroundService
{
    private readonly PipeServerOptions _pipeConnectionOptions;
    private readonly ILogger<PipeServer> _logger;
    private readonly NamedPipeServerStream? _pipeServer;
    private readonly IHostApplicationLifetime _hostApplicationLifetime;
    private readonly IPacketDecoder _packetDecoder;
    private readonly IPacketEncoder _packetEncoder;
    private readonly IDeviceProvider _deviceProvider;

    public PipeServer(IHostApplicationLifetime hostApplicationLifetime,
        IOptions<PipeServerOptions> options,
        IPacketDecoder packetDecoder,
        IPacketEncoder packetEncoder,
        IDeviceProvider deviceProvider,
        ILoggerFactory loggerFactory)
    {
        _pipeConnectionOptions = options.Value;
        ArgumentException.ThrowIfNullOrEmpty(_pipeConnectionOptions.PipeName);

        if (_pipeConnectionOptions.Enabled)
        {
            _pipeServer = new NamedPipeServerStream(_pipeConnectionOptions.PipeName, PipeDirection.InOut, 1, PipeTransmissionMode.Byte,
                 PipeOptions.None);
        }

        _logger = loggerFactory.CreateLogger<PipeServer>();
        _hostApplicationLifetime = hostApplicationLifetime;
        _packetDecoder = packetDecoder;
        _packetEncoder = packetEncoder;
        _deviceProvider = deviceProvider;
        _hostApplicationLifetime.ApplicationStopped.Register(Dispose);
    }

    public override void Dispose()
    {
        base.Dispose();
        if (_pipeServer != null)
        {
            _pipeServer.Close();
            _pipeServer.Dispose();
        }
    }

    public bool IsConnected => _pipeServer != null && _pipeServer.IsConnected;

    protected override async Task ExecuteAsync(CancellationToken cancellationToken)
    {
        while (_pipeConnectionOptions.Enabled && _pipeServer != null)
        {
            _logger.LogInformation($"pipeserver is waiting at pipe={_pipeConnectionOptions.PipeName}...");
            if (!_pipeServer.IsConnected)
            {
                try
                {
                    await _pipeServer.WaitForConnectionAsync(cancellationToken);
                }
                catch (Exception)
                {
                    _pipeServer.Disconnect();
                    _logger.LogWarning($"{_pipeConnectionOptions.PipeName} disconnected!");
                    continue;
                }
            }

            var buffer = ArrayPool<byte>.Shared.Rent(_pipeConnectionOptions.MinBufferSize);
            int read = await _pipeServer.ReadAsync(buffer, 0, buffer.Length, cancellationToken);
            var response = new DeviceServiceInvokeResponse { Code = ErrorCodes.Sys.FAIL };

            try
            {
                if (read > 4)
                {
                    var bodysize = BinaryPrimitives.ReadInt32BigEndian(new ReadOnlySpan<byte>(buffer, 0, 4));
                    if (bodysize > 0 && read >= bodysize - 4)
                    {
                        var deviceServiceInvokeRequest = _packetDecoder.Decode<DeviceServiceInvokeRequest>(new ReadOnlySpan<byte>(buffer, 4, bodysize));
                        _logger.LogInformation($"ReceivedMessage={deviceServiceInvokeRequest.ToJson()}");
                        if (deviceServiceInvokeRequest == null)
                        {
                            response.Message = $"deviceServiceInvokeRequest can't be deserialized!";
                            _logger.LogWarning(response.Message);
                            await SendContent(response);
                        }
                        else if (string.IsNullOrWhiteSpace(deviceServiceInvokeRequest.DeviceId))
                        {
                            response.Message = $"eviceServiceInvokeRequest.DeviceId is empty!";
                            _logger.LogWarning(response.Message);
                            await SendContent(response);
                        }
                        else if (_deviceProvider.GetDevice(deviceServiceInvokeRequest.DeviceId) == null)
                        {
                            response.Message = $"can't find {deviceServiceInvokeRequest.DeviceId} from device provider!";
                            _logger.LogWarning(response.Message);
                            await SendContent(response);
                        }
                        else
                        {
                            var device = _deviceProvider.GetDevice(deviceServiceInvokeRequest.DeviceId);
                            if (device.Commands.TryGetCommand(deviceServiceInvokeRequest.ServiceId, out var serive)
                            && serive != null)
                            {
                                if (deviceServiceInvokeRequest.InvocationKind == ServiceInvocationKind.RequestReply)
                                {
                                    var serviceInvokeResponse = await serive.Invoke(deviceServiceInvokeRequest);
                                    await SendContent(serviceInvokeResponse);
                                }

                                if (deviceServiceInvokeRequest.InvocationKind == ServiceInvocationKind.Post)
                                {
                                    _ = serive.Invoke(deviceServiceInvokeRequest);
                                }
                            }
                            else
                            {
                                response.Message = $"service {deviceServiceInvokeRequest.ServiceId} does not exist!";
                                await SendContent(response);
                            }
                        }
                    }
                }
            }
            finally
            {
                ArrayPool<byte>.Shared.Return(buffer);
            }
        }
    }

    private async Task SendContent(DeviceServiceInvokeResponse response)
    {
        ArgumentNullException.ThrowIfNull(_pipeServer);
        byte[] bytes = _packetEncoder.Encode(response);
        byte[] buffer = ArrayPool<byte>.Shared.Rent(4 + bytes.Length);
        BinaryPrimitives.WriteInt32BigEndian(buffer, bytes.Length);
        Array.Copy(bytes, 0, buffer, 4, bytes.Length);
        await _pipeServer.WriteAsync(buffer);
        await _pipeServer.FlushAsync();
        ArrayPool<byte>.Shared.Return(buffer);
        _logger.LogInformation($"SentContent={response.ToJson()}");
    }
}
