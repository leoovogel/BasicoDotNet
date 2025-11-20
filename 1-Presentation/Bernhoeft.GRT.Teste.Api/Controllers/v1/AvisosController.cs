using Bernhoeft.GRT.Teste.Application.Requests.Commands.v1;
using Bernhoeft.GRT.Teste.Application.Requests.Queries.v1;
using Bernhoeft.GRT.Teste.Application.Responses.Queries.v1;

namespace Bernhoeft.GRT.Teste.Api.Controllers.v1
{
    /// <response code="401">Não Autenticado.</response>
    /// <response code="403">Não Autorizado.</response>
    /// <response code="500">Erro Interno no Servidor.</response>
    [AllowAnonymous]
    [ApiVersion("1.0")]
    [Produces("application/json")]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ProducesResponseType(StatusCodes.Status401Unauthorized, Type = null)]
    [ProducesResponseType(StatusCodes.Status403Forbidden, Type = null)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = null)]
    public class AvisosController : RestApiController
    {
        /// <summary>
        /// Retorna um Aviso por ID.
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns>Aviso.</returns>
        /// <response code="200">Sucesso.</response>
        /// <response code="400">Dados Inválidos.</response>
        /// <response code="404">Aviso Não Encontrado.</response>
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(GetAvisosResponse))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        // [JwtAuthorize(Roles = AuthorizationRoles.CONTRACTLAYOUT_SISTEMA_AVISO_PESQUISAR)]
        public async Task<object> GetAviso([FromRoute] GetAvisoRequest request, CancellationToken cancellationToken)
            => await Mediator.Send(request, cancellationToken);

        /// <summary>
        /// Retorna Todos os Avisos Cadastrados para Tela de Edição.
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns>Lista com Todos os Avisos.</returns>
        /// <response code="200">Sucesso.</response>
        /// <response code="204">Sem Avisos.</response>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IDocumentationRestResult<IEnumerable<GetAvisosResponse>>))]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<object> GetAvisos(CancellationToken cancellationToken)
            => await Mediator.Send(new GetAvisosRequest(), cancellationToken);

        /// <summary>
        /// Cria um novo Aviso.
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns>Aviso criado.</returns>
        /// <response code="201">Criado.</response>
        /// <response code="400">Dados inválidos.</response>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(GetAvisosResponse))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<object> PostAviso([FromBody] CreateAvisoRequest request, CancellationToken cancellationToken)
            => await Mediator.Send(request, cancellationToken);

        /// <summary>
        /// Atualiza apenas a mensagem de um aviso.
        /// </summary>
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(GetAvisosResponse))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<object> PutAviso([FromRoute] int id, [FromBody] UpdateAvisoBodyRequest body, CancellationToken cancellationToken)
        {
            var request = new UpdateAvisoRequest
            {
                Id = id,
                Mensagem = body.Mensagem
            };

            return await Mediator.Send(request, cancellationToken);
        }
    }
}