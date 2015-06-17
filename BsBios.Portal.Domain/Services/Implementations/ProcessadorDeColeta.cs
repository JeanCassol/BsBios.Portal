using System;
using System.Collections.Generic;
using System.Linq;
using BsBios.Portal.Common;
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

            OrdemDeTransporte ordemVinculada = null;

            try
            {

                //verifica se o fornecedor da mercadoria é válido

                int quantidadeDeFornecedoresEncontrados = _fornecedores.BuscaPeloCnpj(conhecimentoDeTransporte.CnpjDoFornecedor).Count();
                if (quantidadeDeFornecedoresEncontrados > 1)
                {
                    throw new Exception(string.Format("Foram encontrados {0} fornecedores com o CNPJ {1}", quantidadeDeFornecedoresEncontrados, conhecimentoDeTransporte.CnpjDoFornecedor));
                }
                
                Fornecedor fornecedor = _fornecedores.Single();

                conhecimentoDeTransporte.AtribuirFornecedorDaMercadoria(fornecedor);

                if (fornecedor == null)
                {
                    return null;
                }

                //verifica se  a transportadora é válida

                int quantidadeDeTransportadorasEncontradas = _fornecedores.BuscaPeloCnpj(conhecimentoDeTransporte.CnpjDaTransportadora).Count();
                if (quantidadeDeTransportadorasEncontradas >  1)
                {
                    throw new Exception(string.Format("Foram encontradas {0} transportadoras com o CNPJ {1}", quantidadeDeTransportadorasEncontradas, conhecimentoDeTransporte.CnpjDaTransportadora));
                }

                Fornecedor transportadora = _fornecedores.BuscaPeloCnpj(conhecimentoDeTransporte.CnpjDaTransportadora).Single();
                conhecimentoDeTransporte.AtribuirTransportadora(transportadora);

                if (transportadora == null)
                {
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

                //estou filtrando em memória pelo número do contrato para não precisar fazer duas consultas ao banco
                List<OrdemDeTransporte> ordensDoContrato = ordensCandidatas.Where(ot => ot.ProcessoDeCotacaoDeFrete.NumeroDoContrato == conhecimentoDeTransporte.NumeroDoContrato).ToList();

                if (ordensDoContrato.Any())
                {
                    ordensCandidatas = ordensDoContrato;
                }

                if (ordensCandidatas.Count == 1)
                {
                    OrdemDeTransporte ordemEncontrada = ordensCandidatas.First();
                    conhecimentoDeTransporte.VincularComOrdem(ordemEncontrada);
                    if (conhecimentoDeTransporte.Status == Enumeradores.StatusDoConhecimentoDeTransporte.Atribuido)
                    {
                        ordemVinculada = conhecimentoDeTransporte.OrdensDeTransporte.FirstOrDefault();    
                    }
                    
                }
                else if (ordensCandidatas.Count > 1)
                {
                    //encontrou mais de uma tem que vincular as ordens candidatas
                    conhecimentoDeTransporte.IndicarOrdensCandidatas(ordensCandidatas);
                }
            }
            catch (Exception exception)
            {
                conhecimentoDeTransporte.InformarErroDeProcessamento(exception.Message);
            }
            return ordemVinculada;
        }
    }
}