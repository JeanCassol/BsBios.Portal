using System.Collections.Generic;

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

        #region overriding members

        protected bool Equals(Incoterm other)
        {
            return string.Equals(Codigo, other.Codigo);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((Incoterm) obj);
        }

        public override int GetHashCode()
        {
            return (Codigo != null ? Codigo.GetHashCode() : 0);
        }

        #endregion

    }
}
