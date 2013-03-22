using System;

namespace BsBios.Portal.Domain.Entities
{
    public class NotaFiscal
    {
        public AgendamentoDeDescarregamento AgendamentoDeDescarregamento { get; protected set; }
        public string Numero { get; protected set; }
        public string Serie { get; protected set; }
        public DateTime DataDeEmissao { get; protected set; }
        public string NomeDoEmitente { get; protected set; }
        public string CnpjDoEmitente { get; protected set; }
        public string NomeDoContratante { get; protected set; }
        public string CnpjDoContratante { get; protected set; }
        public string NumeroDoContrato { get; protected set; }
        public decimal Valor { get; protected set; }
        public decimal Peso { get; protected set; }

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
    }
}
