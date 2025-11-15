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
                );

            //
            // DTO → gRPC Response (nuevo mapeo)
            //
            CreateMap<ClientDto, ClientResponse>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.FullName, opt => opt.MapFrom(src => src.FullName))
                .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email))
                .ForMember(dest => dest.Username, opt => opt.MapFrom(src => src.Username))
                .ForMember(dest => dest.IsActive, opt => opt.MapFrom(src => src.Status == "Active"))
                .ForMember(
                    dest => dest.BirthDate,
                    opt => opt.MapFrom(src => src.BirthDate.ToString())
                )
                .ForMember(dest => dest.PhoneNumber, opt => opt.MapFrom(src => src.PhoneNumber))
                .ForMember(dest => dest.Address, opt => opt.MapFrom(src => src.Address));
        }
    }
}
