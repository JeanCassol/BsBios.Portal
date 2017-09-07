using BsBios.Portal.Common;

namespace BsBios.Portal.Domain.Entities
{
    public class Imposto
    {

        public virtual CotacaoItem CotacaoItem { get; protected set; }
        public virtual Enumeradores.TipoDeImposto Tipo { get; protected set; }
        public virtual decimal BaseDeCalculo { get; protected set; }
        public virtual decimal Aliquota { get; protected set; }
        public virtual decimal Valor { get; protected set; }

        protected  Imposto(){}
        public Imposto(CotacaoItem cotacaoItem, Enumeradores.TipoDeImposto tipo, decimal aliquota, decimal baseDeCalculo)
        {
            CotacaoItem = cotacaoItem;
            Tipo = tipo;
            Aliquota = aliquota;
            BaseDeCalculo = baseDeCalculo;
            CalculaValor();
        }

        private void CalculaValor()
        {
            Valor = Math.Round(BaseDeCalculo * (Aliquota/100),2);
        }

        public virtual void Atualizar(decimal aliquota, decimal baseDeCalculo)
        {
            Aliquota = aliquota;
            BaseDeCalculo = baseDeCalculo;
            CalculaValor();
        }

        #region override members
        protected bool Equals(Imposto other)
        {
            return Equals(CotacaoItem, other.CotacaoItem) && Tipo == other.Tipo;
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return ((CotacaoItem != null ? CotacaoItem.GetHashCode() : 0) * 397) ^ (int)Tipo;
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
