using Bogus;
using ClientsService.Src.Models;
using Microsoft.EntityFrameworkCore;

namespace ClientsService.Src.Data
{
    /// <summary>
    /// Seeding inicial de datos en la base de datos.
    /// </summary>
    public class DataSeeder
    {
        /// <summary>
        /// Método para inicializar el seeding de datos.
        /// </summary>
        public static void Initialize(IServiceProvider serviceProvider)
        {
            var context =
                serviceProvider.GetRequiredService<AppDbContext>()
                ?? throw new InvalidOperationException(
                    "No se pudo obtener el contexto de la base de datos."
                );

            if (context.Clients.Any())
            {
                return;
            }

            context.Database.MigrateAsync();

            var faker = new Faker("es");

            var clients = new List<Client>();
            for (int i = 0; i < 20; i++)
            {
                var client = new Client
                {
                    Id = Guid.NewGuid(),
                    FullName = faker.Name.FullName(),
                    Email = faker.Internet.Email(),
                    Username = faker.Internet.UserName(),
                    BirthDate = DateOnly.FromDateTime(
                        faker.Date.Past(30, DateTime.Now.AddYears(-18))
                    ),
                    Address = faker.Address.FullAddress(),
                    PhoneNumber = faker.Phone.PhoneNumber("+569########"),
                    PasswordHash = BCrypt.Net.BCrypt.HashPassword(
                        faker.Internet.Password(10, false, "", "Aa1!")
                    ),
                    IsActive = true,
                    CreatedAt = DateTime.UtcNow,
                };
                clients.Add(client);
            }

            context.Clients.AddRange(clients);
            context.SaveChanges();
        }
    }
}
