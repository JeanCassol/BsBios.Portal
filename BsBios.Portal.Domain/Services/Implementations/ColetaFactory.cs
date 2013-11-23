using System;
using BsBios.Portal.Common.Exceptions;
using BsBios.Portal.Domain.Entities;
using BsBios.Portal.Domain.Services.Contracts;
using BsBios.Portal.ViewModel;

namespace BsBios.Portal.Domain.Services.Implementations
{
    public class ColetaFactory : IColetaFactory
    {
        public Coleta Construir(ColetaSalvarVm coletaSalvarVm, decimal valorUnitario)
        {
            if (coletaSalvarVm.NotasFiscais.Count == 0)
            {
                throw new ColetaSemNotaFiscalException();
            }
            var coleta = new Coleta(coletaSalvarVm.Placa, coletaSalvarVm.Motorista,Convert.ToDateTime(coletaSalvarVm.DataDePrevisaoDeChegada));

            foreach (var notaFiscalDeColetaVm in coletaSalvarVm.NotasFiscais)
            {
                var notaFiscal = new NotaFiscalDeColeta(notaFiscalDeColetaVm.Numero, notaFiscalDeColetaVm.Serie, notaFiscalDeColetaVm.NumeroDoConhecimento, 
                    Convert.ToDateTime(notaFiscalDeColetaVm.DataDeEmissao), notaFiscalDeColetaVm.Peso, notaFiscalDeColetaVm.Valor);
                
                coleta.AdicionarNotaFiscal(notaFiscal, valorUnitario);
            }

            return coleta;
        }
    }
}