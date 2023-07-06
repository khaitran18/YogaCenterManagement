using FluentValidation;
using Application.Command;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Common.Validation
{
    public class CreateFeedbackCommandValidator : AbstractValidator<CreateFeedbackCommand>
    {
        public CreateFeedbackCommandValidator()
        {
            RuleFor(command => command.Content)
                .NotEmpty().WithMessage("Content is required");
        }
    }
}
