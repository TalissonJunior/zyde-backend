using Zyde.Application.Protocols;

namespace Zyde.WebApi.BackgroundServices;

public class ProtocolListenerHostedService : IHostedService
{
    private readonly ProtocolListener _protocolListener;
    private readonly ILogger<ProtocolListenerHostedService> _logger;

    public ProtocolListenerHostedService(
        ProtocolListener protocolListener,
        ILogger<ProtocolListenerHostedService> logger // Inject ILogger
    )
    {
        _protocolListener = protocolListener;
        _logger = logger;
    }

    public async Task StartAsync(CancellationToken cancellationToken)
    {
        _logger.LogInformation("🚀 [Protocol Listener] Starting...");

        // Development purpose
        //await Task.Run(() => _protocolListener.ProcessFileDataAsync("raw_gps_data.txt"), cancellationToken);
        await Task.Run(() => _protocolListener.ListenOnPorts());
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        _logger.LogInformation("🛑 [Protocol Listener] Stopping...");
        _logger.LogInformation("🔒 [Protocol Listener] Successfully stopped.");
        return Task.CompletedTask;
    }
}
