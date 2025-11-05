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
    public class ClientRepository(AppDbContext appDbContext) : IClientRepository
    {
        private readonly AppDbContext _appDbContext = appDbContext;

        public async Task<Client> CreateClientAsync(Client client)
        {
            _appDbContext.Clients.Add(client);
            await _appDbContext.SaveChangesAsync();
            return client;
        }

        public async Task<bool> EmailExistsAsync(string email)
        {
            return await _appDbContext.Clients.AnyAsync(c => c.Email == email);
        }
    }
}
