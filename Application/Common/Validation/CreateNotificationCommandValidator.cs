using Application.Command;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Common.Validation
{
    public class CreateNotificationCommandValidator : AbstractValidator<CreateNotificationCommand>
    {
        public CreateNotificationCommandValidator()
        {
            RuleFor(c => c.content)
                .NotEmpty().WithMessage("Content must not be empty");
            RuleFor(c => c.scheduleid)
                .GreaterThanOrEqualTo(1).WithMessage("Id must be larger or equal to 1"); 
        }
    }
}
