using System;
using System.Collections.Generic;
using System.Linq;

namespace BsBios.Portal.Domain.Entities
{
    public class NotaFiscalMiro : IAggregateRoot
    {
        #region EqualityMembers

        protected bool Equals(NotaFiscalMiro other)
        {
            return string.Equals(Numero, other.Numero) && string.Equals(CnpjDoFornecedor, other.CnpjDoFornecedor) &&
                   string.Equals(Serie, other.Serie);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((NotaFiscalMiro) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                int hashCode = Numero.GetHashCode();
                hashCode = (hashCode*397) ^ CnpjDoFornecedor.GetHashCode();
                hashCode = (hashCode*397) ^ Serie.GetHashCode();
                return hashCode;
            }
        }

        #endregion


        protected NotaFiscalMiro() { }

        public NotaFiscalMiro(string cnpjDoFornecedor, string numero, string serie)
        {
            CnpjDoFornecedor = cnpjDoFornecedor;
            Numero = numero;
            Serie = serie;
        }

        public virtual string CnpjDoFornecedor { get; protected internal set; }
        public virtual string Numero { get; protected internal set; }
        public virtual string Serie { get; protected internal set; }
        public virtual string MensagemDeProcessamento { get; protected internal set; }

        public virtual void InformarQueOrdemDeTransporteNaoFoiEncontrada()
        {
            this.MensagemDeProcessamento = "Nenhuma ordem de transporte encontrada";
        }

        public virtual void InformarMultiplasOrdensEncontradas(IList<OrdemDeTransporte> ordensDeTransporte)
        {
            this.MensagemDeProcessamento = String.Format("Mais de uma ordem de transporte encontrada: {0}",
                string.Join(", ", ordensDeTransporte.Select(ot => ot.Id)));
        }

        public virtual void InformarQueColetaJaEstaRealizada(int idDaColeta)
        {
            this.MensagemDeProcessamento = String.Format("Coleta já está realizada - Id{0}",idDaColeta);
        }

        public virtual void InformarQueRealizouColeta(Coleta coleta)
        {
            this.MensagemDeProcessamento = string.Format("Realizou coleta - Id: {0}", coleta.Id);
        }
    }
}
