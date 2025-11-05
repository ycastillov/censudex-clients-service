using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ClientsService.Src.Models;

namespace ClientsService.Src.Interfaces
{
    public interface IClientRepository
    {
        Task<bool> EmailExistsAsync(string email);
        Task<Client> CreateClientAsync(Client client);
    }
}
