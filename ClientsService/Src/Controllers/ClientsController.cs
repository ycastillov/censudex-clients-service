using AutoMapper;
using ClientsService.Src.DTOs;
using ClientsService.Src.Helpers;
using ClientsService.Src.Interfaces;
using ClientsService.Src.Models;
using Microsoft.AspNetCore.Mvc;

namespace ClientsService.Src.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    /// <summary>
    /// Controlador para la gestión de clientes.
    /// </summary>
    public class ClientsController(IClientRepository clientRepository, IMapper mapper) : ControllerBase
    {
        private readonly IClientRepository _clientRepository = clientRepository;
        private readonly IMapper _mapper = mapper;

        public async Task<ActionResult<ApiResponse<Client>>> CreateClient([FromBody] ClientCreateDto clientCreateDto)
        {
            if (await _clientRepository.EmailExistsAsync(clientCreateDto.Email))
            {
                return Conflict(new ApiResponse<Client>(false, "El correo electrónico ya está registrado.", null, ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList()));
            }

            clientCreateDto.Password = BCrypt.Net.BCrypt.HashPassword(clientCreateDto.Password);

            var newClient = _mapper.Map<Client>(clientCreateDto);

            var createdClient = await _clientRepository.CreateClientAsync(newClient);
            return CreatedAtAction(nameof(CreateClient), new ApiResponse<Client>(true, "Cliente creado exitosamente.", createdClient));
        }
    }
}