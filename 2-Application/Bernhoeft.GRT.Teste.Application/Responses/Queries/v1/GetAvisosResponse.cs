using Bernhoeft.GRT.ContractWeb.Domain.SqlServer.ContractStore.Entities;

namespace Bernhoeft.GRT.Teste.Application.Responses.Queries.v1
{
    public class GetAvisosResponse
    {
        public int Id { get; set; }
        public string Titulo { get; set; }
        public string Mensagem { get; set; }
        public DateTime DataCriacao { get; set; }
        public DateTime? DataAtualizacao { get; set; }

        public static implicit operator GetAvisosResponse(AvisoEntity entity) => new()
        {
            Id = entity.Id,
            Titulo = entity.Titulo,
            Mensagem = entity.Mensagem,
            DataCriacao = entity.DataCriacao,
            DataAtualizacao = entity.DataAtualizacao
        };
    }
}