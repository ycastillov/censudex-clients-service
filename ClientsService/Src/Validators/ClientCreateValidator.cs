using ClientsService.Src.DTOs;
using FluentValidation;

namespace ClientsService.Src.Validators
{
    /// <summary>
    /// Validador para la creación de un nuevo cliente.
    /// </summary>
    public class ClientCreateValidator : AbstractValidator<ClientCreateDto>
    {
        public ClientCreateValidator()
        {
            RuleFor(x => x.FullName).NotEmpty().MinimumLength(2);
            RuleFor(x => x.Username).NotEmpty().MinimumLength(4);

            RuleFor(x => x.Email)
                .NotEmpty()
                .EmailAddress()
                .Matches(@"^[A-Za-z0-9._%+-]+@censudex\.cl$")
                .WithMessage("El correo debe pertenecer al dominio @censudex.cl");

            RuleFor(x => x.Address).NotEmpty().WithMessage("La dirección no puede estar vacía");

            RuleFor(x => x.PhoneNumber)
                .Matches(@"^\+?56\d{9}$")
                .WithMessage("Número debe ser válido en Chile (+569XXXXXXXX)");

            RuleFor(x => x.BirthDate)
                .Must(Be18OrOlder)
                .WithMessage("El usuario debe ser mayor de 18 años");

            RuleFor(x => x.Password)
                .NotEmpty()
                .WithMessage("La contraseña no puede estar vacía")
                .MinimumLength(8)
                .WithMessage("La contraseña debe tener al menos 8 caracteres")
                .Matches(@"[A-Z]")
                .WithMessage("La contraseña debe contener una letra mayúscula")
                .Matches(@"[a-z]")
                .WithMessage("La contraseña debe contener una letra minúscula")
                .Matches(@"\d")
                .WithMessage("La contraseña debe contener un número")
                .Matches(@"[\W_]")
                .WithMessage("La contraseña debe contener un carácter especial");
        }

        private bool Be18OrOlder(DateOnly birthDate) =>
            birthDate <= DateOnly.FromDateTime(DateTime.UtcNow.AddYears(-18));
    }
}
