namespace BsBios.Portal.Domain.Model
{
    public class Produto: IAggregateRoot
    {
        public virtual string Codigo { get; protected set; }
        public virtual string Descricao { get; protected set; }

        public virtual string Tipo { get; set; }

        protected Produto()
        {
        }

        public Produto(string codigo, string descricao, string tipo)
        {
            Codigo = codigo;
            Descricao = descricao;
            Tipo = tipo;
        }

        public virtual void Atualizar(string novaDescricao, string novoTipo)
        {
            Descricao = novaDescricao;
            Tipo = novoTipo;
        }

    }
}
