using FluentValidation;

namespace Bernhoeft.GRT.Teste.Application.Requests.Commands.v1.Validations;

public class DeleteAvisoRequestValidator : AbstractValidator<DeleteAvisoRequest>
{
    public DeleteAvisoRequestValidator()
    {
        RuleFor(x => x.Id)
            .GreaterThan(0)
            .WithMessage("Id deve ser maior que zero.");
    }
}