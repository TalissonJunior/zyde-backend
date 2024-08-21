using Newtonsoft.Json;
using Zyde.Model;

namespace Zyde.Application.Protocols;

public class GT06Protocol : ProtocolDecoder
{
    private readonly string _decodedString;

    public GT06Protocol(string decodedString) : base(null)
    {
        // Trim any extraneous whitespace or control characters like \r\n
        _decodedString = decodedString.Trim();
    }

    public override Device Decode()
    {
        var device = new Device();

        if (_decodedString.Length < 10 || !_decodedString.StartsWith("*HQ") || !_decodedString.EndsWith("#"))
        {
            return null;
        }

        var parts = _decodedString.TrimEnd('#').Split(',');

        if (parts.Length >= 13)
        {
            var position = new Position();

            string imei = parts[1];
            device.Identifier = imei;
            device.Model = "GT06";

            string protocolVersion = parts[2];
            device.ProtocolVersion = protocolVersion;

            string time = parts[3];
            DateTime dateTime = ParseDateTime(parts[11], time);
            position.Date = dateTime;

            double latitude = ParseCoordinate(parts[5], parts[6], isLatitude: true);
            position.Latitude = (decimal)latitude;

            double longitude = ParseCoordinate(parts[7], parts[8], isLatitude: false);
            position.Longitude = (decimal)longitude;

            double speedKnots = double.Parse(parts[9]);
            position.SpeedInKnots = (decimal)speedKnots;

            double course = double.Parse(parts[10]);
            position.Course = (decimal)course;

            // Parse and display status
            string status = ConvertHexStatus(parts[12]);
            position.Attributes = status;

            device.Positions = new List<Position>() { position };

            return device;
        }
        else
        {
            return null;
        }
    }


   private double ParseCoordinate(string coordinate, string direction, bool isLatitude)
    {
        // Extract degrees and minutes from the coordinate string
        int degreeLength = isLatitude ? 2 : 3; // 2 digits for latitude, 3 for longitude
        
        // Degrees part
        double degrees = double.Parse(coordinate.Substring(0, degreeLength));
        
        // Minutes part
        double minutes = double.Parse(coordinate.Substring(degreeLength).Replace(".", ","));
        
        // Convert to decimal degrees
        double decimalDegrees = degrees + (minutes / 60);

        // Convert to negative if direction is South or West
        if (direction == "S" || direction == "W")
        {
            decimalDegrees = -decimalDegrees;
        }

        return decimalDegrees;
    }

    private DateTime ParseDateTime(string date, string time)
    {
        // Date is in the format "DDMMYY" and time is in "HHMMSS"
        int day = int.Parse(date.Substring(0, 2));
        int month = int.Parse(date.Substring(2, 2));
        int year = int.Parse(date.Substring(4, 2)) + 2000; // Assume 21st century
        int hour = int.Parse(time.Substring(0, 2));
        int minute = int.Parse(time.Substring(2, 2));
        int second = int.Parse(time.Substring(4, 2));

        return new DateTime(year, month, day, hour, minute, second);
    }

    private string ConvertHexStatus(string statusHex)
    {
        try {
            // Convert hex status to a binary string
            string binaryStatus = Convert.ToString(Convert.ToInt32(statusHex, 16), 2).PadLeft(32, '0');

            // Decode specific bits based on the GT06 protocol definition.
            bool gpsTrackingEnabled = binaryStatus[0] == '1';
            bool engineOn = binaryStatus[1] == '1';
            bool charging = binaryStatus[2] == '1';
            bool vibrationAlarm = binaryStatus[3] == '1';
            bool powerCutAlarm = binaryStatus[4] == '1';
            bool lowBatteryAlarm = binaryStatus[5] == '1';
            bool sosAlarm = binaryStatus[6] == '1';
            bool geofenceAlarm = binaryStatus[7] == '1';

            // Handle any additional flags or status bits as per the protocol specification
            // You can continue parsing other bits similarly if needed.

            var jsonData = new  {
                TrackingEnabled = gpsTrackingEnabled,
                EngineOn = engineOn,
                Charging = charging,
                VibrationAlarm = vibrationAlarm,
                PowerCutAlarm = powerCutAlarm,
                LowBatteryAlarm = lowBatteryAlarm,
                GeofenceAlarm = geofenceAlarm,
                SOSAlarm = sosAlarm
            };

            return JsonConvert.SerializeObject(jsonData);
        }
        catch {
            return "{}";
        }
    }
}