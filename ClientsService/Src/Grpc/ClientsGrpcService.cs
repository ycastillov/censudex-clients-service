using AutoMapper;
using ClientsService.Grpc;
using ClientsService.Src.DTOs;
using ClientsService.Src.Extensions;
using ClientsService.Src.Interfaces;
using ClientsService.Src.Models;
using ClientsService.Src.RequestHelpers;
using FluentValidation;
using Grpc.Core;
using Microsoft.EntityFrameworkCore;

public class ClientsGrpcService : ClientsGrpc.ClientsGrpcBase
{
    private readonly IClientRepository _repo;
    private readonly IMapper _mapper;
    private readonly IValidator<ClientCreateDto> _createValidator;
    private readonly IValidator<ClientUpdateDto> _updateValidator;

    public ClientsGrpcService(
        IClientRepository repo,
        IMapper mapper,
        IValidator<ClientCreateDto> createValidator,
        IValidator<ClientUpdateDto> updateValidator
    )
    {
        _repo = repo;
        _mapper = mapper;
        _createValidator = createValidator;
        _updateValidator = updateValidator;
    }

    public override async Task<ClientResponse> CreateClient(
        CreateClientRequest request,
        ServerCallContext context
    )
    {
        try
        {
            if (await _repo.EmailExistsAsync(request.Email))
                throw new RpcException(
                    new Status(StatusCode.AlreadyExists, "Email already exists.")
                );

            var dto = new ClientCreateDto
            {
                FullName = request.FullName,
                Email = request.Email,
                Username = request.Username,
                BirthDate = DateOnly.Parse(request.BirthDate),
                Address = request.Address,
                PhoneNumber = request.PhoneNumber,
                Password = request.Password,
            };

            var validation = await _createValidator.ValidateAsync(dto);
            if (!validation.IsValid)
            {
                throw new RpcException(
                    new Status(
                        StatusCode.InvalidArgument,
                        string.Join(" | ", validation.Errors.Select(e => e.ErrorMessage))
                    )
                );
            }

            var client = _mapper.Map<Client>(dto);
            client.PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password);

            var created = await _repo.CreateClientAsync(client);

            return _mapper.Map<ClientResponse>(created);
        }
        catch (RpcException)
        {
            throw;
        }
        catch (Exception ex)
        {
            throw new RpcException(new Status(StatusCode.Internal, ex.Message));
        }
    }

    public override async Task<ClientsListResponse> GetAllClients(
        GetClientsRequest request,
        ServerCallContext context
    )
    {
        try
        {
            // Convertimos request gRPC → ClientParams
            var filters = new ClientParams
            {
                Username = string.IsNullOrWhiteSpace(request.Username) ? null : request.Username,
                Email = string.IsNullOrWhiteSpace(request.Email) ? null : request.Email,
                FullName = string.IsNullOrWhiteSpace(request.FullName) ? null : request.FullName,
                IsActive = request.IsActive,
            };

            // Obtener IQueryable
            var query = _repo.GetQueryableClients();

            // Aplicar filtros dinámicos
            query = query
                .Filter(filters.IsActive)
                .Search(filters.Username, filters.Email, filters.FullName);

            // Ejecutar query
            var clients = await query.ToListAsync();

            // Mapear resultados
            var response = new ClientsListResponse();
            foreach (var client in clients)
            {
                var dto = _mapper.Map<ClientDto>(client);
                var grpcModel = _mapper.Map<ClientResponse>(dto);
                response.Clients.Add(grpcModel);
            }

            return response;
        }
        catch (Exception ex)
        {
            throw new RpcException(
                new Status(
                    StatusCode.Internal,
                    $"{ex.Message} Error interno al procesar la solicitud"
                )
            );
        }
    }

    public override async Task<ClientResponse> GetClientById(
        GetClientByIdRequest request,
        ServerCallContext context
    )
    {
        try
        {
            var client = await _repo.GetClientByIdAsync(Guid.Parse(request.Id));
            if (client == null)
                throw new RpcException(new Status(StatusCode.NotFound, "Client not found."));

            return _mapper.Map<ClientResponse>(client);
        }
        catch (RpcException)
        {
            throw;
        }
        catch (Exception ex)
        {
            throw new RpcException(new Status(StatusCode.Internal, ex.Message));
        }
    }

    public override async Task<ClientResponse> UpdateClient(
        UpdateClientRequest request,
        ServerCallContext context
    )
    {
        try
        {
            var client = await _repo.GetClientByIdAsync(Guid.Parse(request.Id));
            if (client == null)
                throw new RpcException(new Status(StatusCode.NotFound, "Client not found."));

            var dto = _mapper.Map<ClientUpdateDto>(request);
            var validation = await _updateValidator.ValidateAsync(dto);
            if (!validation.IsValid)
            {
                throw new RpcException(
                    new Status(
                        StatusCode.InvalidArgument,
                        string.Join(" | ", validation.Errors.Select(e => e.ErrorMessage))
                    )
                );
            }

            if (!string.IsNullOrWhiteSpace(request.FullName))
                client.FullName = request.FullName;

            if (!string.IsNullOrWhiteSpace(request.Email))
                client.Email = request.Email;

            if (!string.IsNullOrWhiteSpace(request.Username))
                client.Username = request.Username;

            if (!string.IsNullOrWhiteSpace(request.Address))
                client.Address = request.Address;

            if (!string.IsNullOrWhiteSpace(request.PhoneNumber))
                client.PhoneNumber = request.PhoneNumber;

            if (!string.IsNullOrWhiteSpace(request.Password))
                client.PasswordHash = BCrypt.Net.BCrypt.HashPassword(request.Password);

            var updated = await _repo.UpdateClientAsync(client);

            return _mapper.Map<ClientResponse>(updated);
        }
        catch (RpcException)
        {
            throw;
        }
        catch (Exception ex)
        {
            throw new RpcException(new Status(StatusCode.Internal, ex.Message));
        }
    }

    public override async Task<AuthClientResponse> GetClientByIdentifier(
        GetClientByIdentifierRequest request,
        ServerCallContext context
    )
    {
        var identifier = request.Identifier.ToLower();

        var client = await _repo
            .GetQueryableClients()
            .Where(c => c.Email.ToLower() == identifier || c.Username.ToLower() == identifier)
            .FirstOrDefaultAsync();

        if (client == null)
        {
            return new AuthClientResponse(); // All fields empty → AuthService interprets as null
        }

        return new AuthClientResponse
        {
            Id = client.Id.ToString(),
            Email = client.Email,
            Username = client.Username,
            IsActive = client.IsActive,
            PasswordHash = client.PasswordHash,
            Role = client.Role,
        };
    }

    public override async Task<ClientResponse> DeactivateClient(
        DeactivateClientRequest request,
        ServerCallContext context
    )
    {
        try
        {
            var client = await _repo.GetClientByIdAsync(Guid.Parse(request.Id));
            if (client == null)
                throw new RpcException(new Status(StatusCode.NotFound, "Client not found."));

            await _repo.DeactivateClientAsync(client);

            return _mapper.Map<ClientResponse>(client);
        }
        catch (RpcException)
        {
            throw;
        }
        catch (Exception ex)
        {
            throw new RpcException(new Status(StatusCode.Internal, ex.Message));
        }
    }
}
