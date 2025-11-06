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
    public class ClientsController(IClientRepository clientRepository, IMapper mapper)
        : ControllerBase
    {
        private readonly IClientRepository _clientRepository = clientRepository;
        private readonly IMapper _mapper = mapper;

        /// <summary>
        /// Crea un nuevo cliente.
        /// </summary>
        /// <param name="clientCreateDto">DTO para la creación de un nuevo cliente.</param>
        /// <returns>Respuesta con el cliente creado o un error en caso de operación fallida.</returns>
        [HttpPost]
        public async Task<ActionResult<ApiResponse<Client>>> CreateClient(
            [FromBody] ClientCreateDto clientCreateDto
        )
        {
            if (await _clientRepository.EmailExistsAsync(clientCreateDto.Email))
            {
                return Conflict(
                    new ApiResponse<Client>(
                        false,
                        "El correo electrónico ya está registrado.",
                        null,
                        ModelState
                            .Values.SelectMany(v => v.Errors)
                            .Select(e => e.ErrorMessage)
                            .ToList()
                    )
                );
            }

            clientCreateDto.Password = BCrypt.Net.BCrypt.HashPassword(clientCreateDto.Password);

            var newClient = _mapper.Map<Client>(clientCreateDto);

            var createdClient = await _clientRepository.CreateClientAsync(newClient);
            return CreatedAtAction(
                nameof(CreateClient),
                new ApiResponse<Client>(true, "Cliente creado exitosamente.", createdClient)
            );
        }

        [HttpGet]
        public async Task<ActionResult<ApiResponse<IEnumerable<ClientDto>>>> GetAllClients()
        {
            var clients = await _clientRepository.GetAllClientsAsync();
            if (clients == null || !clients.Any())
            {
                return NotFound(
                    new ApiResponse<IEnumerable<ClientDto>>(
                        false,
                        "No se encontraron clientes.",
                        null
                    )
                );
            }
            var clientsDto = _mapper.Map<IEnumerable<ClientDto>>(clients);
            return Ok(
                new ApiResponse<IEnumerable<ClientDto>>(
                    true,
                    "Lista de clientes obtenida exitosamente.",
                    clientsDto
                )
            );
        }

        /// <summary>
        /// Obtiene un cliente por su ID.
        /// </summary>
        /// <param name="id">ID del cliente.</param>
        /// <returns>Cliente encontrado o null si no existe.</returns>
        [HttpGet("{id}")]
        public async Task<ActionResult<ApiResponse<ClientDto>>> GetClientById(Guid id)
        {
            var client = await _clientRepository.GetClientByIdAsync(id);
            if (client == null)
            {
                return NotFound(new ApiResponse<ClientDto>(false, "Cliente no encontrado.", null));
            }
            var clientDto = _mapper.Map<ClientDto>(client);
            return Ok(
                new ApiResponse<ClientDto>(true, "Cliente obtenido exitosamente.", clientDto)
            );
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<ApiResponse<ClientDto>>> UpdateClient(
            Guid id,
            [FromBody] ClientUpdateDto clientUpdateDto
        )
        {
            var existingClient = await _clientRepository.GetClientByIdAsync(id);
            if (existingClient == null)
            {
                return NotFound(
                    new ApiResponse<ClientDto>(false, "Cliente a actualizar no encontrado.", null)
                );
            }

            // Actualizar los campos del cliente existente
            existingClient.FullName = clientUpdateDto.FullName ?? existingClient.FullName;
            existingClient.Email = clientUpdateDto.Email ?? existingClient.Email;
            existingClient.Username = clientUpdateDto.Username ?? existingClient.Username;
            existingClient.BirthDate = clientUpdateDto.BirthDate ?? existingClient.BirthDate;
            existingClient.Address = clientUpdateDto.Address ?? existingClient.Address;
            existingClient.PhoneNumber = clientUpdateDto.PhoneNumber ?? existingClient.PhoneNumber;

            if (!string.IsNullOrEmpty(clientUpdateDto.Password))
            {
                existingClient.PasswordHash = BCrypt.Net.BCrypt.HashPassword(
                    clientUpdateDto.Password
                );
            }

            var updatedClient = await _clientRepository.UpdateClientAsync(existingClient);
            var updatedClientDto = _mapper.Map<ClientDto>(updatedClient);

            return Ok(
                new ApiResponse<ClientDto>(
                    true,
                    "Cliente actualizado exitosamente.",
                    updatedClientDto
                )
            );
        }
    }
}
