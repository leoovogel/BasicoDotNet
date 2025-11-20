using Bernhoeft.GRT.ContractWeb.Domain.SqlServer.ContractStore.Interfaces.Repositories;
using Bernhoeft.GRT.Core.EntityFramework.Domain.Interfaces;
using Bernhoeft.GRT.Core.Interfaces.Results;
using Bernhoeft.GRT.Core.Models;
using Bernhoeft.GRT.Teste.Application.Requests.Commands.v1;
using Bernhoeft.GRT.Teste.Application.Responses.Queries.v1;
using MediatR;

namespace Bernhoeft.GRT.Teste.Application.Handlers.Commands.v1;

public class UpdateAvisoHandler : IRequestHandler<UpdateAvisoRequest, IOperationResult<GetAvisosResponse>>
{
    private readonly IAvisoRepository _avisoRepository;
    private readonly IContext _context;

    public UpdateAvisoHandler(IAvisoRepository avisoRepository, IContext context)
    {
        _avisoRepository = avisoRepository;
        _context = context;
    }

    public async Task<IOperationResult<GetAvisosResponse>> Handle(UpdateAvisoRequest request, CancellationToken cancellationToken)
    {
        var aviso = await _avisoRepository.GetByIdAsync(request.Id, cancellationToken);

        if (aviso is not { Ativo: true })
            return OperationResult<GetAvisosResponse>.ReturnNotFound();

        aviso.AtualizarMensagem(request.Mensagem);

        await _context.SaveChangesAsync(cancellationToken);

        var response = (GetAvisosResponse)aviso;

        return OperationResult<GetAvisosResponse>.ReturnOk(response);
    }
}