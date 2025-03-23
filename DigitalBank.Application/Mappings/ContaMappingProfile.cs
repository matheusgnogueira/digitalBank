using AutoMapper;
using DigitalBank.Application.DTOs.Conta;
using DigitalBank.Domain.Entities;
using DigitalBank.Util.Extensions;

namespace DigitalBank.Application.Mappings;

public class ContaMappingProfile : Profile
{
    public ContaMappingProfile()
    {
        CreateMap<Conta, ContaRetornoDTO>()
            .ForMember(dest => dest.StatusDescricao, opt => opt.MapFrom(src => src.Status.GetDescription()))
            .ForMember(dest => dest.DataInativacao, opt => opt.MapFrom(src => src.DataInativacao))
            .ForMember(dest => dest.UsuarioInativacao, opt => opt.MapFrom(src => src.UsuarioInativacao));
    }
}
