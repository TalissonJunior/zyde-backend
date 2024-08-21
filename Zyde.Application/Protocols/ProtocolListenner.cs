using System.Text;
using Zyde.Application.Services.CacheService;

namespace Zyde.Application.Protocols;

public class ProtocolListener
{
    private const int Port = 6001;
    private readonly ICacheService cacheService;

    public ProtocolListener(
       ICacheService cacheService
    )
    {
        this.cacheService = cacheService;

        // Update the cache with device data
        cacheService.InitAsync().Wait();
    }

    public async Task ProcessFileDataAsync(string filePath)
    {
        var fileFinalPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", filePath);
        if (!File.Exists(fileFinalPath))
        {
            Console.WriteLine("File not found.");
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
                        GT06Protocol protocol = new GT06Protocol(decodedString);
                        var device = protocol.Decode();

                        if (device == null || (device == null && device.Positions == null)) continue;

                        await cacheService.SetOrUpdateDeviceAsync(device);
                    }
                }
                else
                {
                    Console.WriteLine("File is empty.");
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An error occurred while processing the file: {ex.Message}");
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
