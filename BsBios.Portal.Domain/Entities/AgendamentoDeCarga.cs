using System;
using System.Collections.Generic;
using System.Linq;
using BsBios.Portal.Common;
using BsBios.Portal.Common.Exceptions;
using BsBios.Portal.ViewModel;

namespace BsBios.Portal.Domain.Entities
{
    public abstract class AgendamentoDeCarga: IAggregateRoot
    {
        public virtual int Id { get; protected set; }
        public virtual DateTime Data { get; protected set; }
        public virtual Enumeradores.MaterialDeCarga Material { get; protected set; }
        public virtual string Placa { get; protected set; }
        public virtual bool Realizado { get; protected set; }
        public abstract decimal PesoTotal { get; }


        protected AgendamentoDeCarga()
        {
            Realizado = false;
        }

        protected AgendamentoDeCarga(DateTime data, Enumeradores.MaterialDeCarga material, string placa):this()
        {
            Data = data;
            Material = material;
            Placa = placa;
        }

        public virtual void Realizar()
        {
            Realizado = true;
        }
    }   

    public class AgendamentoDeCarregamento: AgendamentoDeCarga
    {
        public virtual decimal Peso { get; protected set; }

        public override decimal PesoTotal
        {
            get { return Peso; }
        }

        protected AgendamentoDeCarregamento(){}
        internal AgendamentoDeCarregamento(DateTime data, string placa, decimal peso) 
            : base(data, Enumeradores.MaterialDeCarga.Farelo, placa)
        {
            if (peso <=0)
            {
                throw new AgendamentoDeCarregamentoSemPesoException();
            }
            Peso = peso;
        }
    }

    public class AgendamentoDeDescarregamento: AgendamentoDeCarga
    {
        public virtual IList<NotaFiscal> NotasFiscais { get; protected set; }
        protected AgendamentoDeDescarregamento()
        {
            Inicializar();
        }
        internal AgendamentoDeDescarregamento(DateTime data, string placa)
            
            :base(data, Enumeradores.MaterialDeCarga.Soja, placa)
        {
            Inicializar();
        }
        private void Inicializar()
        {
            NotasFiscais = new List<NotaFiscal>();
        }

        public override decimal PesoTotal
        {
            get { return NotasFiscais.Sum(x => x.Peso); }
        }

        public virtual void AdicionarNotaFiscal(NotaFiscalVm notaFiscalVm)
        {
            var notaFiscal = new NotaFiscal(this, notaFiscalVm.Numero, notaFiscalVm.Serie, notaFiscalVm.DataDeEmissao, 
                notaFiscalVm.NomeDoEmitente, notaFiscalVm.CnpjDoEmitente, notaFiscalVm.NomeDoContratante, notaFiscalVm.CnpjDoContratante,
                notaFiscalVm.NumeroDoContrato, notaFiscalVm.Valor, notaFiscalVm.Peso);
            NotasFiscais.Add(notaFiscal);
        }
    }




    

}
