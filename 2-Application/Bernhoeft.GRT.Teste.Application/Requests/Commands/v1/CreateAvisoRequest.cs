using Bernhoeft.GRT.Core.Interfaces.Results;
using Bernhoeft.GRT.Teste.Application.Responses.Queries.v1;
using MediatR;

namespace Bernhoeft.GRT.Teste.Application.Requests.Commands.v1;

public class CreateAvisoRequest : IRequest<IOperationResult<GetAvisosResponse>>
{
    public string Titulo { get; set; } = null!;
    public string Mensagem { get; set; } = null!;
}