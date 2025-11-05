using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ClientsService.Src.DTOs;
using FluentValidation;

namespace ClientsService.Src.Validators
{
    public class ClientCreateValidator : AbstractValidator<ClientCreateDto>
    {
        public ClientCreateValidator()
        {
            RuleFor(x => x.Name).NotEmpty().MinimumLength(2);
            RuleFor(x => x.LastName).NotEmpty().MinimumLength(2);
            RuleFor(x => x.Username).NotEmpty().MinimumLength(4);

            RuleFor(x => x.Email)
                .NotEmpty()
                .EmailAddress()
                .Matches(@"^[A-Za-z0-9._%+-]+@censudex\.cl$")
                .WithMessage("El correo debe pertenecer al dominio @censudex.cl");

            RuleFor(x => x.PhoneNumber)
                .Matches(@"^\+?56\d{9}$")
                .WithMessage("Número debe ser válido en Chile (+569XXXXXXXX)");

            RuleFor(x => x.BirthDate)
                .Must(Be18OrOlder)
                .WithMessage("El usuario debe ser mayor de 18 años");

            RuleFor(x => x.Password)
                .NotEmpty()
                .MinimumLength(8)
                .WithMessage("Debe tener al menos 8 caracteres")
                .Matches(@"[A-Z]")
                .WithMessage("Debe contener una letra mayúscula")
                .Matches(@"[a-z]")
                .WithMessage("Debe contener una letra minúscula")
                .Matches(@"\d")
                .WithMessage("Debe contener un número")
                .Matches(@"[\W_]")
                .WithMessage("Debe contener un carácter especial");
        }

        private bool Be18OrOlder(DateTime birthDate) => birthDate <= DateTime.UtcNow.AddYears(-18);
    }
}
