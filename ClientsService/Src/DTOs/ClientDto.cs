namespace ClientsService.Src.DTOs
{
    public class ClientDto
    {
        /// <summary>
        /// Identificador único del cliente.
        /// </summary>
        public required string Id { get; set; }

        /// <summary>
        /// Nombre completo del cliente.
        /// </summary>
        public required string FullName { get; set; }

        /// <summary>
        /// Correo electrónico del cliente.
        /// </summary>
        public required string Email { get; set; }

        /// <summary>
        /// Nombre de usuario del cliente.
        /// </summary>
        public required string Username { get; set; }

        /// <summary>
        /// Indica si la cuenta del cliente está activa.
        /// </summary>
        public required string Status { get; set; }

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
        /// Fecha de registro del cliente.
        /// </summary>
        public required DateTime CreatedAt { get; set; }

        /// <summary>
        /// Rol del cliente.
        /// </summary>
        public required string Role { get; set; }
    }
}
