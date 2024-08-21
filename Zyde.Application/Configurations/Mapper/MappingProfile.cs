using AutoMapper;
using Zyde.Application.ViewModels.Device;
using Zyde.Model;
using Zyde.Application.ViewModels.Position;
using Zyde.Application.ViewModels.Address;
using Zyde.Application.ViewModels.PositionEvent;
using Zyde.Application.ViewModels.Simcard;
using Zyde.Application.ViewModels.SimcardSubscription;

namespace Zyde.Application.Configurations.Mapper;

public sealed class MappingProfile : Profile
{
    public MappingProfile()
    {

        CreateMap<Device, DeviceView>().ReverseMap();

        CreateMap<Position, PositionView>().ReverseMap();

        CreateMap<Address, AddressView>().ReverseMap();

        CreateMap<PositionEvent, PositionEventView>().ReverseMap();

        CreateMap<Simcard, SimcardView>().ReverseMap();

        CreateMap<SimcardSubscription, SimcardSubscriptionView>().ReverseMap();
    }
}