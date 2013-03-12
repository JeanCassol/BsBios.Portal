namespace BsBios.Portal.Domain.Entities
{
    public class Itinerario:IAggregateRoot
    {
        public virtual string Codigo { get; protected set; }
        public virtual string Descricao { get; protected set;}

        protected Itinerario(){}
        public Itinerario(string codigo, string descricao)
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
