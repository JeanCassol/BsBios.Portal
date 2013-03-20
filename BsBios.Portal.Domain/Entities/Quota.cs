using System;
using BsBios.Portal.Common;

namespace BsBios.Portal.Domain.Entities
{
    public class Quota: IAggregateRoot
    {
        public virtual Enumeradores.FluxoDeCarga FluxoDeCarga { get; protected set; }
        public virtual Fornecedor Transportadora { get; protected set; }
        public virtual string Terminal { get; protected set; }
        public virtual DateTime Data { get; protected set; }
        public virtual decimal Peso { get; protected set; }

        protected Quota(){}

        public Quota(Enumeradores.FluxoDeCarga fluxoDeCarga, Fornecedor transportadora, string terminal, DateTime data, decimal peso)
        {
            FluxoDeCarga = fluxoDeCarga;
            Transportadora = transportadora;
            Terminal = terminal;
            Data = data;
            Peso = peso;
        }

        #region Equality Members

        protected bool Equals(Quota other)
        {
            return FluxoDeCarga == other.FluxoDeCarga && Equals(Transportadora, other.Transportadora) && string.Equals(Terminal, other.Terminal) && Data.Equals(other.Data);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((Quota) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = (int) FluxoDeCarga;
                hashCode = (hashCode*397) ^ (Transportadora != null ? Transportadora.GetHashCode() : 0);
                hashCode = (hashCode*397) ^ (Terminal != null ? Terminal.GetHashCode() : 0);
                hashCode = (hashCode*397) ^ Data.GetHashCode();
                return hashCode;
            }
        }

        #endregion
    }
}