using System.Collections.Generic;

namespace BsBios.Portal.Domain.Entities
{
    public class Fornecedor:IAggregateRoot
    {


        public virtual string Codigo { get; protected set; }
        public virtual string Nome { get; protected set; }
        public virtual string Email { get; protected set; }
        public virtual string Cnpj { get; protected set; }
        public virtual string Municipio { get; protected set; }
        public virtual string Uf { get; protected set; }
        public virtual bool Transportadora { get; protected set; }
        public virtual IList<Produto>  Produtos { get; protected set; }

        protected Fornecedor()
        {
            Produtos = new List<Produto>();
        }

        public Fornecedor(string codigo, string nome, string email, string cnpj, string municipio, string uf, bool transportadora):this()
        {
            Codigo = codigo;
            Nome = nome;
            Email = email;
            Cnpj = cnpj;
            Municipio = municipio;
            Uf = uf;
            Transportadora = transportadora;
        }

        public virtual void Atualizar(string nome, string email, string cnpj, string municipio, string uf, bool transportadora)
        {
            Nome = nome;
            Email = email;
            Cnpj = cnpj;
            Municipio = municipio;
            Uf =uf;
            Transportadora = transportadora;
        }

        #region override
        protected bool Equals(Fornecedor other)
        {
            return string.Equals(Codigo, other.Codigo);
        }

        public override int GetHashCode()
        {
            return (Codigo != null ? Codigo.GetHashCode() : 0);
        }
        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((Fornecedor) obj);
        }
        #endregion
    }
}
