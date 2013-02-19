using System.Collections.Generic;
using System.Linq;

namespace BsBios.Portal.Domain.Model
{
    public class Produto: IAggregateRoot
    {
        public virtual string Codigo { get; protected set; }
        public virtual string Descricao { get; protected set; }
        public virtual string Tipo { get; protected set; }
        public virtual IList<Fornecedor> Fornecedores { get; protected set; }

        protected Produto()
        {
            Fornecedores = new List<Fornecedor>();
        }

        public Produto(string codigo, string descricao, string tipo):this()
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

        public virtual void AdicionarFornecedores(IList<Fornecedor> fornecedores)
        {
             Fornecedores = Fornecedores.Union(fornecedores).ToList();
        }
    }
}
