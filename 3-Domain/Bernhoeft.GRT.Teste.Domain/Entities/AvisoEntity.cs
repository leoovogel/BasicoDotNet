namespace Bernhoeft.GRT.ContractWeb.Domain.SqlServer.ContractStore.Entities
{
    public partial class AvisoEntity
    {
        public int Id { get; private set; }
        public bool Ativo { get; set; } = true;
        public string Titulo { get; set; }
        public string Mensagem { get; set; }

        public DateTime DataCriacao { get; private set; } = DateTime.UtcNow;
        public DateTime? DataAtualizacao { get; private set; }

        public void AtualizarMensagem(string novaMensagem)
        {
            Mensagem = novaMensagem;
            DataAtualizacao = DateTime.UtcNow;
        }

        public void DesativarAviso()
        {
            Ativo = false;
            DataAtualizacao = DateTime.UtcNow;
        }
    }
}