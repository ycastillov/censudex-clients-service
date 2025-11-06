using ClientsService.Src.Models;

namespace ClientsService.Src.Extensions
{
    public static class ClientExtensions
    {
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
