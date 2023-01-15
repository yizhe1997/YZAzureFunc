using AutoMapper;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Extensions;
using YZAzureFunc.Function.Domain.Models.Fno;

namespace YZAzureFunc.Function.Domain.Providers.Fno
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<FnoResponse, Response>()
                .ForMember(c => c.data, opt => opt.MapFrom(src => src.responseEntities.IsNullOrDefault() ? src.responseEntity : src.responseEntities));
        }
    }
}
