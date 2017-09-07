using System;

namespace BsBios.Portal.Domain.Entities
{
    public class HistoricoDePreco
    {
        public virtual int Id { get; set; }
        public virtual DateTime DataHora { get; protected set; }
        public virtual decimal Valor { get; protected set; }

        public HistoricoDePreco(decimal valor)
        {
            DataHora = DateTime.Now;
            Valor = valor;
        }
        protected HistoricoDePreco(){}
    }
}