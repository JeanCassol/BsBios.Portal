using System;

namespace BsBios.Portal.Domain.Entities
{
    public class NotaFiscal
    {
        public virtual AgendamentoDeDescarregamento AgendamentoDeDescarregamento { get; protected set; }
        public virtual string Numero { get; protected set; }
        public virtual string Serie { get; protected set; }
        public virtual DateTime DataDeEmissao { get; protected set; }
        public virtual string NomeDoEmitente { get; protected set; }
        public virtual string CnpjDoEmitente { get; protected set; }
        public virtual string NomeDoContratante { get; protected set; }
        public virtual string CnpjDoContratante { get; protected set; }
        public virtual string NumeroDoContrato { get; protected set; }
        public virtual decimal Valor { get; protected set; }
        public virtual decimal Peso { get; protected set; }

        protected NotaFiscal(){}

        internal NotaFiscal(AgendamentoDeDescarregamento agendamentoDeDescarregamento, string numero, string serie, DateTime dataDeEmissao, 
            string nomeDoEmitente, string cnpjDoEmitente, string nomeDoContratante, string cnpjDoContratante, string numeroDoContrato,
            decimal valor, decimal peso)
        {
            AgendamentoDeDescarregamento = agendamentoDeDescarregamento;
            Numero = numero;
            Serie = serie;
            DataDeEmissao = dataDeEmissao;
            NomeDoEmitente = nomeDoEmitente;
            CnpjDoEmitente = cnpjDoEmitente;
            NomeDoContratante = nomeDoContratante;
            CnpjDoContratante = cnpjDoContratante;
            NumeroDoContrato = numeroDoContrato;
            Valor = valor;
            Peso = peso;
        }

        #region Equality Members

        protected bool Equals(NotaFiscal other)
        {
            return string.Equals(Numero, other.Numero) && string.Equals(Serie, other.Serie) && string.Equals(CnpjDoEmitente, other.CnpjDoEmitente);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((NotaFiscal) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                int hashCode = (Numero != null ? Numero.GetHashCode() : 0);
                hashCode = (hashCode*397) ^ (Serie != null ? Serie.GetHashCode() : 0);
                hashCode = (hashCode*397) ^ (CnpjDoEmitente != null ? CnpjDoEmitente.GetHashCode() : 0);
                return hashCode;
            }
        }

        #endregion
    }
}
