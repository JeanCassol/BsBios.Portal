namespace BsBios.Portal.Domain.Model
{
    public class CondicaoDePagamento: IAggregateRoot
    {
        public virtual string Codigo { get; protected set; }
        public virtual string Descricao { get; protected set;}

        protected CondicaoDePagamento(){}
        public CondicaoDePagamento(string codigo, string descricao)
        {
            Codigo = codigo;
            Descricao = descricao;
        }

        public virtual void AtualizarDescricao(string descricao)
        {
            Descricao = descricao;
        }
    }
}
