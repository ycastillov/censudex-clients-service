using ClientsService.Src.DTOs;
using FluentValidation;

namespace ClientsService.Src.Validators
{
    /// <summary>
    /// Validador para la actualización de un cliente existente.
    /// </summary>
    public class ClientUpdateValidator : AbstractValidator<ClientUpdateDto>
    {
        public ClientUpdateValidator()
        {
            RuleFor(x => x.FullName).MinimumLength(2).When(x => !string.IsNullOrEmpty(x.FullName));
            RuleFor(x => x.Username).MinimumLength(4).When(x => !string.IsNullOrEmpty(x.Username));

            RuleFor(x => x.Email)
                .EmailAddress()
                .Matches(@"^[A-Za-z0-9._%+-]+@censudex\.cl$")
                .WithMessage("El correo debe pertenecer al dominio @censudex.cl")
                .When(x => !string.IsNullOrEmpty(x.Email));

            RuleFor(x => x.Address)
                .NotEmpty()
                .WithMessage("La dirección no puede estar vacía")
                .When(x => !string.IsNullOrEmpty(x.Address));

            RuleFor(x => x.PhoneNumber)
                .Matches(@"^\+?56\d{9}$")
                .WithMessage("Número debe ser válido en Chile (+569XXXXXXXX)")
                .When(x => !string.IsNullOrEmpty(x.PhoneNumber));

            RuleFor(x => x.BirthDate)
                .Must(Be18OrOlder)
                .WithMessage("El usuario debe ser mayor de 18 años")
                .When(x => x.BirthDate.HasValue);

            RuleFor(x => x.Password)
                .MinimumLength(8)
                .WithMessage("La contraseña debe tener al menos 8 caracteres")
                .Matches(@"[A-Z]")
                .WithMessage("La contraseña debe contener una letra mayúscula")
                .Matches(@"[a-z]")
                .WithMessage("La contraseña debe contener una letra minúscula")
                .Matches(@"\d")
                .WithMessage("La contraseña debe contener un número")
                .Matches(@"[\W_]")
                .WithMessage("La contraseña debe contener un carácter especial")
                .When(x => !string.IsNullOrEmpty(x.Password));
        }

        private bool Be18OrOlder(DateOnly? birthDate) =>
            birthDate.HasValue
            && birthDate.Value <= DateOnly.FromDateTime(DateTime.UtcNow.AddYears(-18));
    }
}
