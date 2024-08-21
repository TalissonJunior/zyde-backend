using Coffee.Application.Configurations;

namespace Zyde.Application.Configurations;

public class AppSettings : CoffeeAppSettings
{
    public new ConnectionStrings ConnectionStrings { get; set; }
    public ProtocolListenerSettings ProtocolListener { get; set; }
}


public class ConnectionStrings {
    public string DefaultConnection { get; set; }    
    public string LogConnection { get; set; }
    public string RedisConnection { get; set; }
}

public class ProtocolListenerSettings {
    public string Ports { get; set; }    
    public string LogConnection { get; set; }
    public string RedisConnection { get; set; }
}

