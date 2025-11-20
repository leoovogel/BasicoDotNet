using Bernhoeft.GRT.ContractWeb.Domain.SqlServer.ContractStore.Entities;
using Bernhoeft.GRT.ContractWeb.Domain.SqlServer.ContractStore.Interfaces.Repositories;
using Bernhoeft.GRT.Core.EntityFramework.Domain.Interfaces;
using Bernhoeft.GRT.Core.Interfaces.Results;
using Bernhoeft.GRT.Core.Models;
using Bernhoeft.GRT.Teste.Application.Requests.Commands.v1;
using Bernhoeft.GRT.Teste.Application.Responses.Queries.v1;
using MediatR;

namespace Bernhoeft.GRT.Teste.Application.Handlers.Commands.v1;

public class CreateAvisoHandler : IRequestHandler<CreateAvisoRequest, IOperationResult<GetAvisosResponse>>
{
    private readonly IAvisoRepository _avisoRepository;
    private readonly IContext _context;

    public CreateAvisoHandler(IAvisoRepository avisoRepository, IContext context)
    {
        _avisoRepository = avisoRepository;
        _context = context;
    }

    public async Task<IOperationResult<GetAvisosResponse>> Handle(CreateAvisoRequest request, CancellationToken cancellationToken)
    {
        var entity = new AvisoEntity
        {
            Titulo = request.Titulo,
            Mensagem = request.Mensagem,
            Ativo = true,
        };

        await _avisoRepository.AddAsync(entity, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);

        return OperationResult<GetAvisosResponse>.ReturnCreated();
    }
}