namespace ClientsService.Src.DTOs
{
    /// <summary>
    /// DTO para la creación de un nuevo cliente.
    /// </summary>
    public class ClientCreateDto
    {
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
        /// Contraseña del cliente.
        /// </summary>
        public required string Password { get; set; }
    }
}
