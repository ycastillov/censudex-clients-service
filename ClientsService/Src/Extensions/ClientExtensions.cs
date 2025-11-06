using ClientsService.Src.Models;

namespace ClientsService.Src.Extensions
{
    /// <summary>
    /// Extensiones para consultas IQueryable de Client.
    /// </summary>
    public static class ClientExtensions
    {
        /// <summary>
        /// Filtra los clientes por su estado activo/inactivo.
        /// </summary>
        /// <param name="query">Query que contiene a los clientes.</param>
        /// <param name="isActive">true para Active, false para Inactive.</param>
        /// <returns>Query de clientes con filtros aplicados.</returns>
        public static IQueryable<Client> Filter(this IQueryable<Client> query, bool? isActive)
        {
            if (isActive.HasValue)
            {
                query = isActive.Value
                    ? query.Where(c => c.IsActive)
                    : query.Where(c => !c.IsActive);
            }
            return query;
        }

        /// <summary>
        /// Realiza una búsqueda parcial en los campos Username, Email y FullName.
        /// </summary>
        /// <param name="query">Query que contiene a los clientes.</param>
        /// <param name="username">Nombre de usuario para búsqueda parcial.</param>
        /// <param name="email">Correo electrónico para búsqueda parcial.</param>
        /// <param name="fullName">Nombre completo para búsqueda parcial.</param>
        /// <returns>Query de clientes con búsqueda aplicada.</returns>
        public static IQueryable<Client> Search(
            this IQueryable<Client> query,
            string? username,
            string? email,
            string? fullName
        )
        {
            if (!string.IsNullOrWhiteSpace(username))
            {
                var lowerUsername = username.ToLower();
                query = query.Where(c => c.Username.ToLower().Contains(lowerUsername));
            }
            if (!string.IsNullOrWhiteSpace(email))
            {
                var lowerEmail = email.ToLower();
                query = query.Where(c => c.Email.ToLower().Contains(lowerEmail));
            }
            if (!string.IsNullOrWhiteSpace(fullName))
            {
                var lowerFullName = fullName.ToLower();
                query = query.Where(c => c.FullName.ToLower().Contains(lowerFullName));
            }

            return query;
        }
    }
}
