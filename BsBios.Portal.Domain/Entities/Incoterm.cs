namespace BsBios.Portal.Domain.Entities
{
    public class Incoterm: IAggregateRoot
    {
        public virtual string Codigo { get; protected set; }
        public virtual string Descricao { get; set; }

        protected Incoterm(){}

        public Incoterm(string codigo, string descricao)
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
