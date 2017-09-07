using System.Collections.Generic;
using System.Linq;

namespace BsBios.Portal.Domain.Entities
{
    public class Produto: IAggregateRoot
    {
        public virtual string Codigo { get; protected set; }
        public virtual string Descricao { get; protected set; }
        public virtual string Tipo { get; protected set; }
        public virtual IList<Fornecedor> Fornecedores { get; protected set; }
        public virtual bool MaterialPrima
        {
            get { return Tipo.ToUpper().Equals("ROH"); }
        }

        public virtual bool NaoEstocavel
        {
            get { return Tipo.ToUpper().Equals("NLAG"); }
        }

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
