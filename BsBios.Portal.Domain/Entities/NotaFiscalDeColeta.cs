using System;

namespace BsBios.Portal.Domain.Entities
{
    public class NotaFiscalDeColeta
    {
        protected NotaFiscalDeColeta(){}

        public NotaFiscalDeColeta(string numero, string serie, string numeroDoConhecimento, DateTime dataDeEmissao, decimal peso, decimal valor)
        {
            Numero = numero;
            Serie = serie;
            NumeroDoConhecimento = numeroDoConhecimento;
            DataDeEmissao = dataDeEmissao;
            Peso = peso;
            Valor = valor;
        }

        public virtual int Id { get; protected set; }
        public virtual string Numero { get; protected set; }
        public virtual string NumeroDoConhecimento { get; set; }

        public virtual string Serie { get; protected set; }
        public virtual DateTime DataDeEmissao { get; protected set; }
        public virtual decimal Peso { get; protected set; }
        public virtual decimal Valor { get; protected set; }

        public virtual void Atualizar(string numero, string serie, string numeroDoConhecimento, DateTime dataDeEmissao, decimal peso, decimal valor)
        {
            Numero = numero;
            Serie = serie;
            NumeroDoConhecimento = numeroDoConhecimento;
            DataDeEmissao = dataDeEmissao;
            Peso = peso;
            Valor = valor;
        }
    }
}