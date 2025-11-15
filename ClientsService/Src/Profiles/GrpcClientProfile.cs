using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using ClientsService.Grpc;
using ClientsService.Src.DTOs;
using ClientsService.Src.Models;

namespace ClientsService.Src.Profiles
{
    public class GrpcClientProfile : Profile
    {
        public GrpcClientProfile()
        {
            //
            // ClientUpdateRequest (gRPC) → DTO actualización
            //
            CreateMap<UpdateClientRequest, ClientUpdateDto>()
                .ForMember(
                    dest => dest.BirthDate,
                    opt =>
                        opt.MapFrom(src =>
                            string.IsNullOrEmpty(src.BirthDate)
                                ? (DateOnly?)null
                                : DateOnly.Parse(src.BirthDate)
                        )
                );
            //
            // Modelo → gRPC Response
            //
            CreateMap<Client, ClientResponse>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id.ToString()))
                .ForMember(
                    dest => dest.BirthDate,
                    opt => opt.MapFrom(src => src.BirthDate.ToString())
                )
                .ForMember(dest => dest.PasswordHash, opt => opt.MapFrom(src => src.PasswordHash));
        }
    }
}
