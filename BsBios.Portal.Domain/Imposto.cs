using BsBios.Portal.Common;

namespace BsBios.Portal.Domain
{
    public class Imposto
    {
        public Enumeradores.TipoDeImposto Tipo{ get; protected set; }
        public decimal Aliquota { get; protected set; }
        public decimal Valor { get; protected set; }

        protected  Imposto(){}
        public Imposto(Enumeradores.TipoDeImposto tipo, decimal aliquota, decimal valor)
        {
            Tipo = tipo;
            Aliquota = aliquota;
            Valor = valor;
        }

        public virtual void Atualizar(decimal aliquota, decimal valor)
        {
            Aliquota = aliquota;
            Valor = valor;
        }
    }
}
