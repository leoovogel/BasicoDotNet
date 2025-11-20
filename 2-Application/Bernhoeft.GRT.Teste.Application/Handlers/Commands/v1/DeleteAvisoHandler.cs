using Bernhoeft.GRT.ContractWeb.Domain.SqlServer.ContractStore.Interfaces.Repositories;
using Bernhoeft.GRT.Core.EntityFramework.Domain.Interfaces;
using Bernhoeft.GRT.Core.Interfaces.Results;
using Bernhoeft.GRT.Core.Models;
using Bernhoeft.GRT.Teste.Application.Requests.Commands.v1;
using MediatR;

namespace Bernhoeft.GRT.Teste.Application.Handlers.Commands.v1;

public class DeleteAvisoHandler : IRequestHandler<DeleteAvisoRequest, IOperationResult<object>>
{
    private readonly IAvisoRepository _avisoRepository;
    private readonly IContext _context;

    public DeleteAvisoHandler(IAvisoRepository avisoRepository, IContext context)
    {
        _avisoRepository = avisoRepository;
        _context = context;
    }

    public async Task<IOperationResult<object>> Handle(DeleteAvisoRequest request, CancellationToken cancellationToken)
    {
        var aviso = await _avisoRepository.GetByIdAsync(request.Id, cancellationToken);

        if (aviso is not { Ativo: true })
            return OperationResult<object>.ReturnNotFound();

        aviso.Ativo = false;

        await _context.SaveChangesAsync(cancellationToken);

        return OperationResult<object>.ReturnNoContent();
    }
}