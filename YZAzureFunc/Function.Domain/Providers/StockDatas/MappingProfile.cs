using AutoMapper;
using YZAzureFunc.Function.Domain.Models.StockDatas;

namespace YZAzureFunc.Function.Domain.Providers.StockDatas
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            // Index
            CreateMap<FinhubStockData, StockData>()
                .ForMember(c => c.Open, opt => opt.MapFrom(src => src.o))
                .ForMember(c => c.High, opt => opt.MapFrom(src => src.h))
                .ForMember(c => c.Low, opt => opt.MapFrom(src => src.l))
                .ForMember(c => c.Current, opt => opt.MapFrom(src => src.c))
                .ForMember(c => c.PreviousClose, opt => opt.MapFrom(src => src.pc));
        }
    }
}
