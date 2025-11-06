using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ClientsService.Src.Data;
using ClientsService.Src.Interfaces;
using ClientsService.Src.Models;
using Microsoft.EntityFrameworkCore;

namespace ClientsService.Src.Repositories
{
    /// <summary>
    /// Repositorio para la gestión de clientes.
    /// </summary>
    public class ClientRepository(AppDbContext appDbContext) : IClientRepository
    {
        private readonly AppDbContext _appDbContext = appDbContext;

        /// <summary>
        /// Crea un nuevo cliente en la base de datos.
        /// </summary>
        /// <param name="client">Cliente a crear.</param>
        /// <returns>Cliente creado.</returns>
        public async Task<Client> CreateClientAsync(Client client)
        {
            _appDbContext.Clients.Add(client);
            await _appDbContext.SaveChangesAsync();
            return client;
        }

        /// <summary>
        /// Verifica si un correo electrónico ya está registrado.
        /// </summary>
        /// <param name="email">Correo electrónico a verificar</param>
        /// <returns>True si es que existe, caso contrario retorna False.</returns>
        public async Task<bool> EmailExistsAsync(string email)
        {
            return await _appDbContext.Clients.AnyAsync(c => c.Email == email);
        }

        /// <summary>
        /// Obtiene la lista de todos los clientes.
        /// </summary>
        /// <returns>Retorna la lista de clientes.</returns>
        public async Task<IEnumerable<Client>> GetAllClientsAsync()
        {
            return await _appDbContext.Clients.ToListAsync();
        }

        /// <summary>
        /// Obtiene un cliente por su ID.
        /// </summary>
        /// <param name="id">ID del cliente.</param>
        /// <returns>Cliente encontrado o null si no existe.</returns>
        public async Task<Client?> GetClientByIdAsync(Guid id)
        {
            return await _appDbContext.Clients.FirstOrDefaultAsync(c => c.Id == id);
        }

        /// <summary>
        /// Actualiza un cliente existente.
        /// </summary>
        /// <param name="client">Cliente a actualizar.</param>
        /// <returns>Cliente actualizado.</returns>
        public async Task<Client> UpdateClientAsync(Client client)
        {
            _appDbContext.Clients.Update(client);
            await _appDbContext.SaveChangesAsync();
            return client;
        }

        /// <summary>
        /// Desactiva un cliente.
        /// </summary>
        /// <param name="client">Cliente a desactivar.</param>
        /// <returns>True si se desactivó correctamente, false en caso contrario.</returns>
        public async Task DeactivateClientAsync(Client client)
        {
            client.IsActive = false;
            _appDbContext.Clients.Update(client);
            await _appDbContext.SaveChangesAsync();
        }

        /// <summary>
        /// Obtiene clientes en una query
        /// </summary>
        /// <returns>IQueryable de clientes.</returns>
        public IQueryable<Client> GetQueryableClients()
        {
            return _appDbContext.Clients.AsQueryable();
        }
    }
}
