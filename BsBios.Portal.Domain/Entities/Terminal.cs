namespace BsBios.Portal.Domain.Entities
{
    public class Terminal: IAggregateRoot
    {
        protected Terminal(){}

        public Terminal(string codigo, string nome,string cidade)
        {
            Codigo = codigo;
            Nome = nome;
            Cidade = cidade;
        }

        public virtual string Codigo { get; set; }
        public virtual string Nome { get; set; }
        public virtual string Cidade { get; set; }

        public virtual void Atualizar(string nome, string cidade)
        {
            Nome = nome;
            Cidade = cidade;
        }
    }
}
