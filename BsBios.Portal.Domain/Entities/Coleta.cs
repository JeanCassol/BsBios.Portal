using System;
using System.Collections.Generic;
using System.Linq;
using BsBios.Portal.Common;
using BsBios.Portal.ViewModel;

namespace BsBios.Portal.Domain.Entities
{
    public class Coleta
    {

        protected Coleta()
        {
            NotasFiscais = new List<NotaFiscalDeColeta>();
        }

        internal Coleta(string placa, string motorista, DateTime dataDePrevisaoDeChegada):this()
        {
            Placa = placa;
            Motorista = motorista;
            DataDePrevisaoDeChegada = dataDePrevisaoDeChegada;
        }

        public virtual int Id  { get; protected set; }
        public virtual bool Realizado { get; protected set; }
        public virtual string Placa { get; protected set; }
        public virtual string Motorista { get; protected set; }

        public virtual DateTime DataDePrevisaoDeChegada { get; protected  set; }

        public virtual decimal Peso { get; set; }

        public virtual decimal ValorDoFrete { get; set; }

        public virtual IList<NotaFiscalDeColeta> NotasFiscais { get; protected set; }

        private void CalcularValores(decimal valorUnitario)
        {
            Peso = NotasFiscais.Sum(x => x.Peso);

            ValorDoFrete = Peso * valorUnitario;
            
        }

        public virtual void Atualizar(ColetaSalvarVm coletaSalvarVm, decimal valorUnitario)
        {
            Placa = coletaSalvarVm.Placa;
            Motorista = coletaSalvarVm.Motorista;
            DataDePrevisaoDeChegada = Convert.ToDateTime(coletaSalvarVm.DataDePrevisaoDeChegada);

            AtualizarNotasFiscais(coletaSalvarVm.NotasFiscais);

            CalcularValores(valorUnitario);

        }

        public virtual void AdicionarNotaFiscal(NotaFiscalDeColeta notaFiscalDeColeta, decimal valorUnitario)
        {
            NotasFiscais.Add(notaFiscalDeColeta);

            CalcularValores(valorUnitario);
        }

        private void AtualizarNotasFiscais(IList<NotaFiscalDeColetaVm> notasFiscaisVm)
        {
            var notasParaExcluir = NotasFiscais.Where(notaFiscalAdicionada => notasFiscaisVm
                .All(notaInformada => !notaInformada.Id.HasValue || notaInformada.Id != notaFiscalAdicionada.Id)).ToList();

            foreach (var notaFiscalDeColeta in notasParaExcluir)
            {
                NotasFiscais.Remove(notaFiscalDeColeta);
            }

            var notasParaAlterar = (from notaFiscalAdicionada in NotasFiscais
                join notaFiscalInformada in notasFiscaisVm
                    on notaFiscalAdicionada.Id equals notaFiscalInformada.Id
                where notaFiscalInformada.Id.HasValue
                select new
                {
                    NotaAdicionada = notaFiscalAdicionada,
                    NotaInformada = notaFiscalInformada
                });

            foreach (var notaParaAlterar in notasParaAlterar)
            {
                var notaAdicionada = notaParaAlterar.NotaAdicionada;
                var notaInformada = notaParaAlterar.NotaInformada;

                notaAdicionada.Atualizar(notaInformada.Numero, notaInformada.Serie, Convert.ToDateTime(notaInformada.DataDeEmissao), 
                    notaInformada.Peso, notaInformada.Valor);

            }

            var notasParaAdicionar = notasFiscaisVm.Where(x => !x.Id.HasValue);

            foreach (var notaFiscalDeColetaVm in notasParaAdicionar)
            {
                var notaFiscalDeColeta = new NotaFiscalDeColeta(notaFiscalDeColetaVm.Numero, notaFiscalDeColetaVm.Serie,
                    Convert.ToDateTime(notaFiscalDeColetaVm.DataDeEmissao), notaFiscalDeColetaVm.Peso, notaFiscalDeColetaVm.Valor);

                NotasFiscais.Add(notaFiscalDeColeta);
            }


        }
    }
}
