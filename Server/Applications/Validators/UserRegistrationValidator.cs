using Core.Models.Request;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Applications.Validators
{
    public class UserRegistrationValidator : AbstractValidator<UserRegisterRequest>
    {
        public UserRegistrationValidator()
        {
            RuleFor(x => x.Email)
                .Cascade(CascadeMode.Stop)
                .NotEmpty()
                    .WithMessage("Email is required.")
                .MaximumLength(255)
                    .WithMessage("Email cannot exceed 255 characters.")
                .EmailAddress()
                    .WithMessage("Invalid email format.");

            RuleFor(x => x.Password)
                .Cascade(CascadeMode.Stop)
                .NotEmpty()
                    .WithMessage("Password is required.")
                .MinimumLength(8)
                    .WithMessage("Password must be at least 8 characters long.")
                .MaximumLength(64)
                    .WithMessage("Password cannot exceed 64 characters.")
                .Matches("[A-Z]")
                    .WithMessage("Password must contain at least one uppercase letter.")
                .Matches("[a-z]")
                    .WithMessage("Password must contain at least one lowercase letter.")
                .Matches("[0-9]")
                    .WithMessage("Password must contain at least one digit.")
                .Must(password => !password.Contains(' '))
                    .WithMessage("Password cannot contain spaces.");

            RuleFor(x => x.Name)
                .Cascade(CascadeMode.Stop)
                .NotEmpty()
                    .WithMessage("User Name is required.")
                .MinimumLength(3)
                    .WithMessage("User Name must be at least 3 characters long.")
                .MaximumLength(64)
                    .WithMessage("User Name cannot exceed 64 characters.")
                .Matches(@"^[a-zA-Zа-яА-ЯіІїЇєЄґҐ0-9_\- ]+$")
                    .WithMessage("User Name contains invalid characters.");
        }
    }
}
