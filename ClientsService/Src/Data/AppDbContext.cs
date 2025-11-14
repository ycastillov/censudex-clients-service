using ClientsService.Src.Models;
using Microsoft.EntityFrameworkCore;

namespace ClientsService.Src.Data
{
    public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
    {
        public DbSet<Client> Clients { get; set;} = null!;
    }
}
