using System.Text;
using Zyde.Application.Services.CacheService;
using Microsoft.Extensions.Logging;
using System.Net.Sockets;
using System.Net;
using Zyde.Application.Configurations;
using Microsoft.Extensions.Options;

namespace Zyde.Application.Protocols;

public class ProtocolListener
{
    private readonly ICacheService cacheService;
    private readonly ILogger<ProtocolListener> logger;
    private readonly int[] ports;

    public ProtocolListener(
        ICacheService cacheService,
        ILogger<ProtocolListener> logger,
        IOptions<AppSettings> appSettings
    )
    {
        this.cacheService = cacheService;
        this.logger = logger;
        ports = appSettings.Value.ProtocolListener.Ports
            .Split(",")
            .Select(p => int.Parse(p.Trim()))
            .ToArray();

        // Update the cache with device data
        cacheService.InitAsync().Wait();
    }

    // For Testing purpose
    public async Task ProcessFileDataAsync(string filePath)
    {
        var fileFinalPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", filePath);
        if (!File.Exists(fileFinalPath))
        {
            logger.LogError("❌ File not found.");
            return;
        }

        try
        {
            var lines = await File.ReadAllLinesAsync(fileFinalPath);

            foreach (var line in lines)
            {
                if (!string.IsNullOrWhiteSpace(line))
                {
                    byte[] byteArray = ConvertAsciiHexStringToByteArray(line);
                    string decodedString = Encoding.ASCII.GetString(byteArray).Trim();

                    if (IsGT06Protocol(decodedString))
                    {
                        logger.LogInformation("ℹ️ Found GT06 GPS data: {DecodedString}", decodedString);

                        GT06Protocol protocol = new GT06Protocol(decodedString);
                        var device = protocol.Decode();

                        if (device == null || (device == null && device.Positions == null))
                        {
                            logger.LogWarning("⚠️ Failed to decode GT06 GPS data: {DecodedString}", decodedString);
                            continue;
                        }

                        await cacheService.SetOrUpdateDeviceAsync(device);
                    }
                }
                else
                {
                    logger.LogWarning("⚠️ File is empty.");
                }
            }
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "❗ An error occurred while processing the file.");
        }
    }

    public void ListenOnPorts()
    {
        foreach (var port in ports)
        {
            _ = Task.Run(() => ListenOnPortAsync(port));
        }
    }

    private async Task ListenOnPortAsync(int port)
    {
        TcpListener listener = null;

        try
        {
            listener = new TcpListener(IPAddress.Any, port);
            listener.Start();
            logger.LogInformation("🚀 Listening on port {Port}", port);

            while (true)
            {
                using var client = await listener.AcceptTcpClientAsync();
                using var networkStream = client.GetStream();

                byte[] buffer = new byte[4096];
                int bytesRead = await networkStream.ReadAsync(buffer, 0, buffer.Length);

                string decodedString = Encoding.ASCII.GetString(buffer, 0, bytesRead).Trim();
                logger.LogInformation("ℹ️ Received data on port {Port}: {DecodedString}", port, decodedString);

                if (IsGT06Protocol(decodedString))
                {
                    GT06Protocol protocol = new GT06Protocol(decodedString);
                    var device = protocol.Decode();

                    if (device != null && device.Positions != null)
                    {
                        await cacheService.SetOrUpdateDeviceAsync(device);
                    }
                    else
                    {
                        logger.LogWarning("⚠️ Failed to decode GT06 GPS data: {DecodedString}", decodedString);
                    }
                }
            }
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "❗ An error occurred while listening on port {Port}.", port);
        }
        finally
        {
            listener?.Stop();
            logger.LogInformation("🛑 Stopped listening on port {Port}", port);
        }
    }

    private bool IsGT06Protocol(string decodedString)
    {
        return decodedString.StartsWith("*HQ") && decodedString.EndsWith("#");
    }

    private byte[] ConvertAsciiHexStringToByteArray(string hexString)
    {
        int length = hexString.Length / 2;
        byte[] bytes = new byte[length];
        for (int i = 0; i < length; i++)
        {
            bytes[i] = Convert.ToByte(hexString.Substring(2 * i, 2), 16);
        }
        return bytes;
    }
}
