using System;
using System.Collections.Generic;
using System.Linq;
using BsBios.Portal.Common.Exceptions;
using BsBios.Portal.ViewModel;

namespace BsBios.Portal.Domain.Entities
{
    public abstract class AgendamentoDeCarga: IAggregateRoot
    {
        public virtual int Id { get; protected set; }
        public virtual Quota Quota { get; protected set; }
        public virtual string Placa { get; protected set; }
        public virtual bool Realizado { get; protected set; }
        public abstract decimal PesoTotal { get; }

        protected AgendamentoDeCarga()
        {
            Realizado = false;
        }

        protected AgendamentoDeCarga(Quota quota, string placa):this()
        {
            Quota = quota;
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
        internal AgendamentoDeCarregamento(Quota quota, string placa, decimal peso) 
            : base(quota, placa)
        {
            if (peso <=0)
            {
                throw new AgendamentoDeCarregamentoSemPesoException();
            }
            Peso = peso;
        }

        public virtual void Atualizar(AgendamentoDeCarregamentoCadastroVm agendamentoDeCarregamentoCadastroVm)
        {
            Placa = agendamentoDeCarregamentoCadastroVm.Placa;
            Peso = agendamentoDeCarregamentoCadastroVm.Peso;
        }
    }

    public class AgendamentoDeDescarregamento: AgendamentoDeCarga
    {
        public virtual IList<NotaFiscal> NotasFiscais { get; protected set; }
        protected AgendamentoDeDescarregamento()
        {
            Inicializar();
        }
        internal AgendamentoDeDescarregamento(Quota quota, string placa)
            
            :base(quota, placa)
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
            var notaFiscal = new NotaFiscal(this, notaFiscalVm.Numero, notaFiscalVm.Serie, Convert.ToDateTime(notaFiscalVm.DataDeEmissao), 
                notaFiscalVm.NomeDoEmitente, notaFiscalVm.CnpjDoEmitente, notaFiscalVm.NomeDoContratante, notaFiscalVm.CnpjDoContratante,
                notaFiscalVm.NumeroDoContrato, notaFiscalVm.Valor, notaFiscalVm.Peso);
            NotasFiscais.Add(notaFiscal);
        }

        public void Atualizar(AgendamentoDeDescarregamentoSalvarVm agendamentoDeDescarregamentoCadastroVm)
        {
            Placa = agendamentoDeDescarregamentoCadastroVm.Placa;

            IList<NotaFiscal> notasParaRemover = NotasFiscais.Where(nfSalva => agendamentoDeDescarregamentoCadastroVm.NotasFiscais
                .All(nfCadastro => nfCadastro.Numero != nfSalva.Numero || nfCadastro.Serie != nfSalva.Serie || nfCadastro.CnpjDoEmitente != nfSalva.CnpjDoEmitente )).ToList();

            foreach (var notaFiscal in notasParaRemover)
            {
                NotasFiscais.Remove(notaFiscal);
            }

            var query = (from nfSalva in NotasFiscais
                     join nfCadastro in agendamentoDeDescarregamentoCadastroVm.NotasFiscais
                         on new {nfSalva.Numero, nfSalva.Serie, nfSalva.CnpjDoEmitente} equals
                         new {nfCadastro.Numero, nfCadastro.Serie, nfCadastro.CnpjDoEmitente}
                     select new {nfSalva, nfCadastro}
                    );

            foreach (var registro in query)
            {
                registro.nfSalva.Alterar(registro.nfCadastro);
            }

            IList<NotaFiscalVm> notasParaAdicionar = agendamentoDeDescarregamentoCadastroVm.NotasFiscais.Where(nfCadastro => NotasFiscais
                .All(nfSalva => nfCadastro.Numero != nfSalva.Numero || nfCadastro.Serie != nfSalva.Serie || nfCadastro.CnpjDoEmitente != nfSalva.CnpjDoEmitente )).ToList();

            foreach (var notaFiscalVm in notasParaAdicionar)
            {
                AdicionarNotaFiscal(notaFiscalVm);
            }

            if (NotasFiscais.Count == 0)
            {
                throw new AgendamentoDeDescarregamentoSemNotaFiscalException();
            }

        }
    }




    

}
