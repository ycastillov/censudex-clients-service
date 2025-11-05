using System.ComponentModel.DataAnnotations;

namespace ClientsService.Src.Models
{
    /// <summary>
    /// Representa un cliente en el sistema.
    /// </summary>
    public class Client
    {
        [Key]
        public required Guid Id { get; set; }

        /// <summary>
        /// Nombre del cliente.
        /// </summary>
        public required string Name { get; set; }

        /// <summary>
        /// Apellido del cliente.
        /// </summary>
        public required string LastName { get; set; }

        /// <summary>
        /// Correo electrónico del cliente.
        /// </summary>
        public required string Email { get; set; }

        /// <summary>
        /// Nombre de usuario del cliente.
        /// </summary>
        public required string Username { get; set; }

        /// <summary>
        /// Fecha de nacimiento del cliente.
        /// </summary>
        public required DateOnly BirthDate { get; set; }

        /// <summary>
        /// Dirección del cliente.
        /// </summary>
        public required string Address { get; set; }

        /// <summary>
        /// Número de teléfono del cliente.
        /// </summary>
        public required string PhoneNumber { get; set; }

        /// <summary>
        /// Contraseña del cliente (hash).
        /// </summary>
        public required string PasswordHash { get; set; }

        /// <summary>
        /// Indica si la cuenta del cliente está activa.
        /// </summary>
        public bool IsActive { get; set; } = true;

        /// <summary>
        /// Fecha de registro del cliente.
        /// </summary>
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
