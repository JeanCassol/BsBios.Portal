using System.Collections.Generic;
using System.Linq;
using BsBios.Portal.Domain.Entities;
using BsBios.Portal.Domain.Repositories;
using BsBios.Portal.Domain.Services.Contracts;

namespace BsBios.Portal.Domain.Services.Implementations
{
    public class ProcessadorDeColeta : IProcessadorDeColeta
    {
        private readonly IOrdensDeTransporte _ordensDeTransporte;
        private readonly IFornecedores _fornecedores;

        public ProcessadorDeColeta(IOrdensDeTransporte ordensDeTransporte, IFornecedores fornecedores)
        {
            _ordensDeTransporte = ordensDeTransporte;
            _fornecedores = fornecedores;
        }

        public OrdemDeTransporte Processar(ConhecimentoDeTransporte conhecimentoDeTransporte)
        {

            //verifica se o fornecedor da mercadoria � v�lido
            Fornecedor fornecedor = _fornecedores.BuscaPeloCnpj(conhecimentoDeTransporte.CnpjDoFornecedor);

            if (fornecedor == null)
            {
                conhecimentoDeTransporte.InformarErroDeProcessamento("Fornecedor da Mercadoria n�o encontrado");
                return null;
            }

            //verifica se  a transportadora � v�lida
            Fornecedor transportadora = _fornecedores.BuscaPeloCnpj(conhecimentoDeTransporte.CnpjDaTransportadora);

            if (transportadora == null)
            {
                conhecimentoDeTransporte.InformarErroDeProcessamento("Transportadora n�o encontrada");
                return null;
            }

            //busca ordens candidatas;
            IList<OrdemDeTransporte> ordensCandidatas = _ordensDeTransporte
                .ComPeriodoDeValidadeContendoAData(conhecimentoDeTransporte.DataDeEmissao)
                .ComColetaAberta()
                .DoFornecedorDaMercadoria(conhecimentoDeTransporte.CnpjDoFornecedor)
                .DaTransportadora(conhecimentoDeTransporte.CnpjDaTransportadora)
                .IncluirProcessoDeCotacao()
                .List();

            //estou filtrando em mem�ria pelo n�mero do contrato para n�o precisar fazer duas consultas ao banco
            List<OrdemDeTransporte> ordensDoContrato = ordensCandidatas.Where(ot => ot.ProcessoDeCotacaoDeFrete.NumeroDoContrato == conhecimentoDeTransporte.NumeroDoContrato).ToList();

            if (ordensDoContrato.Any())
            {
                ordensCandidatas = ordensDoContrato;
            }

            OrdemDeTransporte ordemEncontrada = null;

            if (ordensCandidatas.Count == 1)
            {
                ordemEncontrada = ordensCandidatas.First();
                conhecimentoDeTransporte.VincularComOrdem(ordemEncontrada);
            }
            else if (ordensCandidatas.Count > 1)
            {
                //encontrou mais de uma tem que vincular as ordens candidatas
                conhecimentoDeTransporte.IndicarOrdensCandidatas(ordensCandidatas);
            }

            return ordemEncontrada;
        }
    }
}