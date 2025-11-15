using AutoMapper;
using ClientsService.Grpc;
using ClientsService.Src.Interfaces;
using ClientsService.Src.Models;
using Grpc.Core;

public class ClientsGrpcService : ClientsGrpc.ClientsGrpcBase
{
    private readonly IClientRepository _repo;
    private readonly IMapper _mapper;

    public ClientsGrpcService(IClientRepository repo, IMapper mapper)
    {
        _repo = repo;
        _mapper = mapper;
    }

    public override async Task<ClientResponse> CreateClient(
        CreateClientRequest request,
        ServerCallContext context
    )
    {
        if (await _repo.EmailExistsAsync(request.Email))
            throw new RpcException(new Status(StatusCode.AlreadyExists, "Email already exists."));

        var client = new Client
        {
            Id = Guid.NewGuid(),
            FullName = request.FullName,
            Email = request.Email,
            Username = request.Username,
            BirthDate = DateOnly.Parse(request.BirthDate),
            Address = request.Address,
            PhoneNumber = request.PhoneNumber,
            PasswordHash = BCrypt.Net.BCrypt.HashPassword(request.Password),
            IsActive = true,
            CreatedAt = DateTime.UtcNow,
        };

        await _repo.CreateClientAsync(client);

        return ToGrpcClient(client);
    }

    public override async Task<ClientsListResponse> GetAllClients(
        Empty request,
        ServerCallContext context
    )
    {
        var clients = await _repo.GetAllClientsAsync();

        var response = new ClientsListResponse();
        response.Clients.AddRange(clients.Select(c => ToGrpcClient(c)));

        return response;
    }

    public override async Task<ClientResponse> GetClientById(
        GetClientByIdRequest request,
        ServerCallContext context
    )
    {
        var client = await _repo.GetClientByIdAsync(Guid.Parse(request.Id));

        if (client == null)
            throw new RpcException(new Status(StatusCode.NotFound, "Client not found."));

        return ToGrpcClient(client);
    }

    public override async Task<ClientResponse> UpdateClient(
        UpdateClientRequest request,
        ServerCallContext context
    )
    {
        var client = await _repo.GetClientByIdAsync(Guid.Parse(request.Id));
        if (client == null)
            throw new RpcException(new Status(StatusCode.NotFound, "Client not found."));

        client.FullName = request.FullName ?? client.FullName;
        client.Email = request.Email ?? client.Email;
        client.Username = request.Username ?? client.Username;
        client.Address = request.Address ?? client.Address;
        client.PhoneNumber = request.PhoneNumber ?? client.PhoneNumber;

        if (!string.IsNullOrWhiteSpace(request.Password))
            client.PasswordHash = BCrypt.Net.BCrypt.HashPassword(request.Password);

        await _repo.UpdateClientAsync(client);

        return ToGrpcClient(client);
    }

    public override async Task<ClientResponse> DeactivateClient(
        DeactivateClientRequest request,
        ServerCallContext context
    )
    {
        var client = await _repo.GetClientByIdAsync(Guid.Parse(request.Id));
        if (client == null)
            throw new RpcException(new Status(StatusCode.NotFound, "Client not found."));

        await _repo.DeactivateClientAsync(client);

        return ToGrpcClient(client);
    }

    public override async Task<ClientResponse> GetClientByIdentifier(
        GetClientByIdentifierRequest request,
        ServerCallContext context
    )
    {
        var identifier = request.Identifier;

        var client = _repo
            .GetQueryableClients()
            .FirstOrDefault(c => c.Email == identifier || c.Username == identifier);

        if (client == null)
            throw new RpcException(new Status(StatusCode.NotFound, "Client not found."));

        return ToGrpcClient(client);
    }

    private ClientResponse ToGrpcClient(Client c) =>
        new ClientResponse
        {
            Id = c.Id.ToString(),
            FullName = c.FullName,
            Email = c.Email,
            Username = c.Username,
            BirthDate = c.BirthDate.ToString(),
            Address = c.Address,
            PhoneNumber = c.PhoneNumber,
            PasswordHash = c.PasswordHash,
            IsActive = c.IsActive,
        };
}
