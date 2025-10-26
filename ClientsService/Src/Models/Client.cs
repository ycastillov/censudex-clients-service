using System.ComponentModel.DataAnnotations;

namespace ClientsService.Src.Models
{
    public class Client
    {
        [Key]
        public required Guid Id { get; set; }
        public required string Name { get; set; }
        public required string LastName { get; set; }
        public required string Email { get; set; }
        public required string Username { get; set; }
        public required DateTime BirthDate { get; set; }
        public required string Address { get; set; }
        public required string PhoneNumber { get; set; }
        public required string PasswordHash { get; set; }
        public bool IsActive { get; set; } = true;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
