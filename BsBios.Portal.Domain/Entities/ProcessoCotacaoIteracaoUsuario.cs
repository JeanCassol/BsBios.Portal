namespace BsBios.Portal.Domain.Entities
{
    public class ProcessoCotacaoIteracaoUsuario: IAggregateRoot
    {
        public virtual int IdFornecedorParticipante { get; protected set; }
        public virtual bool VisualizadoPeloFornecedor { get; protected set; }

        protected ProcessoCotacaoIteracaoUsuario(){}
        public ProcessoCotacaoIteracaoUsuario(int idFornecedorParticipante)
        {
            IdFornecedorParticipante = idFornecedorParticipante;
        }
        public virtual void FornecedorVisualizou()
        {
            VisualizadoPeloFornecedor = true;
        }
    }

}
