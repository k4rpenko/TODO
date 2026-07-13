using Core.Models.Request;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Applications.Validators
{
    public class UserLoginValidator : AbstractValidator<UserLoginRequest>
    {
        public UserLoginValidator()
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

        }
    }
}
