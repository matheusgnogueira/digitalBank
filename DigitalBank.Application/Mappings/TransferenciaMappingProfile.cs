using AutoMapper;
using DigitalBank.Application.DTOs.Transferencia;
using DigitalBank.Domain.Entities;

namespace DigitalBank.Application.Mappings;

public class TransferenciaMappingProfile : Profile
{
    public TransferenciaMappingProfile()
    {
        CreateMap<Transferencia, TransferenciaRetornoDTO>().ReverseMap();
    }
}
