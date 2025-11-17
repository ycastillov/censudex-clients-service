using AutoMapper;
using ClientsService.Grpc;
using ClientsService.Src.DTOs;
using ClientsService.Src.Models;

namespace ClientsService.Src.Profiles
{
    /// <summary>
    /// Perfil de AutoMapper para la entidad Client.
    /// </summary>
    public class ClientProfile : Profile
    {
        public ClientProfile()
        {
            //
            // DTO → Modelo
            //
            CreateMap<ClientCreateDto, Client>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
                .ForMember(dest => dest.PasswordHash, opt => opt.MapFrom(src => src.Password))
                .ForMember(dest => dest.IsActive, opt => opt.MapFrom(src => true))
                .ForMember(dest => dest.Role, opt => opt.MapFrom(src => "CLIENT"));

            //
            // Modelo → DTO general
            //
            CreateMap<Client, ClientDto>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id.ToString()))
                .ForMember(
                    dest => dest.Status,
                    opt => opt.MapFrom(src => src.IsActive ? "Active" : "Inactive")
                );
        }
    }
}
