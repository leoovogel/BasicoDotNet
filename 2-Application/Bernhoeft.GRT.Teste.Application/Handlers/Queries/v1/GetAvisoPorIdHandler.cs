using Bernhoeft.GRT.ContractWeb.Domain.SqlServer.ContractStore.Interfaces.Repositories;
using Bernhoeft.GRT.Core.EntityFramework.Domain.Interfaces;
using Bernhoeft.GRT.Core.Enums;
using Bernhoeft.GRT.Core.Interfaces.Results;
using Bernhoeft.GRT.Core.Models;
using Bernhoeft.GRT.Teste.Application.Requests.Queries.v1;
using Bernhoeft.GRT.Teste.Application.Responses.Queries.v1;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace Bernhoeft.GRT.Teste.Application.Handlers.Queries.v1
{
    public class GetAvisoPorIdHandler : IRequestHandler<GetAvisoByIdRequest, IOperationResult<GetAvisosResponse>>
    {
        private readonly IServiceProvider _serviceProvider;

        private IContext _context => _serviceProvider.GetRequiredService<IContext>();
        private IAvisoRepository _avisoRepository => _serviceProvider.GetRequiredService<IAvisoRepository>();

        public GetAvisoPorIdHandler(IServiceProvider serviceProvider) => _serviceProvider = serviceProvider;

        public async Task<IOperationResult<GetAvisosResponse>> Handle(GetAvisoByIdRequest byIdRequest, CancellationToken cancellationToken)
        {
            var result = await _avisoRepository.ObterAvisoPorIdAsync(byIdRequest.Id, TrackingBehavior.NoTracking, cancellationToken);

            return result is null
                ? OperationResult<GetAvisosResponse>.ReturnNoContent()
                : OperationResult<GetAvisosResponse>.ReturnOk(result);
        }
    }
}