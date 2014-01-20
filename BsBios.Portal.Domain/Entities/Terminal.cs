namespace BsBios.Portal.Domain.Entities
{
    public class Terminal: IAggregateRoot
    {
        protected Terminal(){}

        public Terminal(string codigo, string descricao)
        {
            Codigo = codigo;
            Descricao = descricao;
        }

        public virtual string Codigo { get; set; }
        public virtual string Descricao { get; set; }

    }
}
