namespace ClientsService.Src.DTOs
{
    /// <summary>
    /// DTO para la actualización de un cliente existente.
    /// </summary>
    public class ClientUpdateDto
    {
        /// <summary>
        /// Nombre completo del cliente.
        /// </summary>
        public string? FullName { get; set; }

        /// <summary>
        /// Correo electrónico del cliente.
        /// </summary>
        public string? Email { get; set; }

        /// <summary>
        /// Nombre de usuario del cliente.
        /// </summary>
        public string? Username { get; set; }

        /// <summary>
        /// Fecha de nacimiento del cliente.
        /// </summary>
        public DateOnly? BirthDate { get; set; }

        /// <summary>
        /// Dirección del cliente.
        /// </summary>
        public string? Address { get; set; }

        /// <summary>
        /// Número de teléfono del cliente.
        /// </summary>
        public string? PhoneNumber { get; set; }

        /// <summary>
        /// Contraseña del cliente.
        /// </summary>
        public string? Password { get; set; }
    }
}
