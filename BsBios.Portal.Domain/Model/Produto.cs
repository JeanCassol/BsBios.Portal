namespace BsBios.Portal.Domain.Model
{
    public class Produto: IAggregateRoot
    {
        public virtual string Codigo { get; protected set; }
        public virtual string Descricao { get; protected set; }

        protected Produto()
        {
        }

        public Produto(string codigo, string descricao)
        {
            Codigo = codigo;
            Descricao = descricao;
        }

        public virtual void AtualizaDescricao(string novaDescricao)
        {
            Descricao = novaDescricao;
        }

    }
}
