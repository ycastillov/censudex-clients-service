using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ClientsService.Src.DTOs
{
    public class ClientDto
    {
        public required string Id { get; set; }
        public required string FullName { get; set; }
        public required string Email { get; set; }
        public required string Username { get; set; }
        public required string Status { get; set; }
        public required DateOnly BirthDate { get; set; }
        public required string Address { get; set; }
        public required string PhoneNumber { get; set; }
        public required DateTime CreatedAt { get; set; }
    }
}
