using AutoMapper;
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
            // Mapeo de DTO → Modelo
            CreateMap<ClientCreateDto, Client>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.IsActive, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
                .ForMember(dest => dest.PasswordHash, opt => opt.MapFrom(src => src.Password));
        }
    }
}
