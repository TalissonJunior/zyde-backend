using Zyde.Application.Protocols;

namespace Zyde.WebApi.BackgroundServices;

public class ProtocolListenerHostedService : IHostedService
{
    private readonly ProtocolListener _protocolListener;
    private readonly ILogger<ProtocolListenerHostedService> _logger;

    public ProtocolListenerHostedService(
        ProtocolListener protocolListener, 
        ILogger<ProtocolListenerHostedService> logger
    )
    {
        _protocolListener = protocolListener;
        _logger = logger;
    }

    public async Task StartAsync(CancellationToken cancellationToken)
    {
        _logger.LogInformation("🚀 [Protocol Listener] Starting...");

        // You can start listening here
        await Task.Run(() => _protocolListener.ProcessFileDataAsync("raw_gps_data.txt"), cancellationToken);

        _logger.LogInformation("✅ [Protocol Listener] Successfully started and processing data from 'raw_gps_data.txt'.");
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        _logger.LogInformation("🛑 [Protocol Listener] Stopping...");
        
        // Handle any cleanup if necessary

        _logger.LogInformation("🔒 [Protocol Listener] Successfully stopped.");
        return Task.CompletedTask;
    }
}