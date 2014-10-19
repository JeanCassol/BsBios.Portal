namespace BsBios.Portal.Domain.Entities
{
    public class NotaFiscalDeConhecimentoDeTransporte
    {

        protected NotaFiscalDeConhecimentoDeTransporte() { }

        internal NotaFiscalDeConhecimentoDeTransporte(string chaveEletronica, string numero, string serie)
        {
            ChaveEletronica = chaveEletronica;
            Numero = numero;
            Serie = serie;
        }

        public virtual string ChaveEletronica { get; protected internal set; }
        public virtual string Numero { get; protected internal set; }
        public virtual string Serie { get; protected internal set; }

    }
}
