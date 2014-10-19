using System.Collections.Generic;
using System.Linq;
using BsBios.Portal.Common.Exceptions;
using BsBios.Portal.Domain.Entities;
using BsBios.Portal.Domain.Repositories;
using BsBios.Portal.Domain.Services.Contracts;

namespace BsBios.Portal.Domain.Services.Implementations
{
    public class ProcessadorDeNotaFiscalMiro: IProcessadorDeNotaFiscalMiro
    {

        private readonly IOrdensDeTransporte _ordensDeTransporte;

        public ProcessadorDeNotaFiscalMiro(IOrdensDeTransporte ordensDeTransporte)
        {
            _ordensDeTransporte = ordensDeTransporte;
        }

        public OrdemDeTransporte Processar(NotaFiscalMiro notaFiscal)
        {
            //buscar coleta a partir dos dados
            IList<OrdemDeTransporte> ordensDeTransporte = _ordensDeTransporte
                .DoFornecedorDaMercadoria(notaFiscal.CnpjDoFornecedor)
                .ContendoNotaFiscalDeColeta(notaFiscal.Numero, notaFiscal.Serie).List();

            OrdemDeTransporte ordemDeTransporte = null;

            switch (ordensDeTransporte.Count)
            {
                case 0:
                    notaFiscal.InformarQueOrdemDeTransporteNaoFoiEncontrada();
                    break;
                case 1:
                {
                    ordemDeTransporte = ordensDeTransporte.Single();

                    try
                    {
                        Coleta coletaRealizada = ordemDeTransporte.RealizarColeta(notaFiscal.Numero, notaFiscal.Serie);
                        notaFiscal.InformarQueRealizouColeta(coletaRealizada);
                    }
                    catch (RealizacaoDeColetaRealizadaException exception)
                    {
                        notaFiscal.InformarQueColetaJaEstaRealizada(exception.IdDaColeta);
                    }
                }
                    break;
                default:
                    notaFiscal.InformarMultiplasOrdensEncontradas(ordensDeTransporte);
                    break;
            }

            return ordemDeTransporte;

        }
    }
}