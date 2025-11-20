using FluentValidation;

namespace Bernhoeft.GRT.Teste.Application.Requests.Commands.v1.Validations;

public class UpdateAvisoRequestValidator : AbstractValidator<UpdateAvisoRequest>
{
    public UpdateAvisoRequestValidator()
    {
        RuleFor(x => x.Id)
            .GreaterThan(0)
            .WithMessage("Id deve ser maior que zero.");

        RuleFor(x => x.Mensagem)
            .NotEmpty()
            .WithMessage("A mensagem é obrigatória.");
    }
}