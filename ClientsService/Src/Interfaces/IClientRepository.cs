using ClientsService.Src.Models;

namespace ClientsService.Src.Interfaces
{
    /// <summary>
    /// Interfaz del repositorio para la gestión de clientes.
    /// </summary>
    public interface IClientRepository
    {
        // Verifica si un correo electrónico ya está registrado.
        Task<bool> EmailExistsAsync(string email);

        // Crea un nuevo cliente.
        Task<Client> CreateClientAsync(Client client);

        // Obtener la lista de todos los clientes.
        Task<IEnumerable<Client>> GetAllClientsAsync();

        // Obtener un cliente por su ID.
        Task<Client?> GetClientByIdAsync(Guid id);

        // Actualizar un cliente existente.
        Task<Client> UpdateClientAsync(Client client);

        // Desactivar un cliente.
        Task DeactivateClientAsync(Client client);
    }
}
