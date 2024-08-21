using Zyde.Model;

namespace Zyde.Application.Protocols;

public abstract class ProtocolDecoder
{   
    protected byte[] Data { get; private set; }

    protected ProtocolDecoder(byte[] data)
    {
        Data = data;
    }

    public abstract Device Decode();
}