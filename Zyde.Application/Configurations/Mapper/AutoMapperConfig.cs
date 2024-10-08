using AutoMapper;

namespace Zyde.Application.Configurations.Mapper;

public sealed class AutoMapperConfig
{
    public static IMapper Initialize()
    {
        var mapperConfig = new MapperConfiguration(mc =>
        {
            mc.AddProfile(new MappingProfile());
        });
        
        return mapperConfig.CreateMapper();
    }
}