using BsBios.Portal.Common;

namespace BsBios.Portal.Domain.Entities
{
    public class Imposto
    {

        public virtual Cotacao Cotacao { get; protected set; }
        public virtual Enumeradores.TipoDeImposto Tipo { get; protected set; }
        public virtual decimal Aliquota { get; protected set; }
        public virtual decimal Valor { get; protected set; }

        protected  Imposto(){}
        public Imposto(Cotacao cotacao, Enumeradores.TipoDeImposto tipo, decimal aliquota, decimal valor)
        {
            Cotacao = cotacao;
            Tipo = tipo;
            Aliquota = aliquota;
            Valor = valor;
        }

        public virtual void Atualizar(decimal aliquota, decimal valor)
        {
            Aliquota = aliquota;
            Valor = valor;
        }

        #region override members
        protected bool Equals(Imposto other)
        {
            return Equals(Cotacao, other.Cotacao) && Tipo == other.Tipo;
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return ((Cotacao != null ? Cotacao.GetHashCode() : 0) * 397) ^ (int)Tipo;
            }
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((Imposto) obj);
        }
        #endregion
    }
}
