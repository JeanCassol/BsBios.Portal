using System.Linq;
using BsBios.Portal.Common;
using BsBios.Portal.Domain.Entities;
using BsBios.Portal.Infra.Services.Contracts;
using BsBios.Portal.ViewModel;

namespace BsBios.Portal.Infra.Services.Implementations
{
    public class ProcessoDeCotacaoDeMaterialFechamentoComunicacaoSap : IProcessoDeCotacaoDeMaterialFechamentoComunicacaoSap
    {
        private readonly IComunicacaoSap<ProcessoDeCotacaoDeMaterialFechamentoComunicacaoSapVm> _comunicacaoSap;

        public ProcessoDeCotacaoDeMaterialFechamentoComunicacaoSap(IComunicacaoSap<ProcessoDeCotacaoDeMaterialFechamentoComunicacaoSapVm> comunicacaoSap)
        {
            _comunicacaoSap = comunicacaoSap;
        }

        public void EfetuarComunicacao(ProcessoDeCotacaoDeMaterial processo,
                                                     ProcessoDeCotacaoDeMaterialFechamentoInfoVm fechamentoVm)
        {
            foreach (var fornecedorParticipante in processo.FornecedoresParticipantes)
            {
                var cotacao = (CotacaoMaterial) fornecedorParticipante.Cotacao.CastEntity();
                var itens = new ListaProcessoDeCotacaoDeMaterialFechamentoItemVm();
                itens.AddRange((from item in cotacao.Itens
                let cotacaoItemMaterial = (CotacaoMaterialItem) item.CastEntity()
                let processoItemMaterial = (ProcessoDeCotacaoDeMaterialItem) cotacaoItemMaterial.ProcessoDeCotacaoItem.CastEntity()
                select new ProcessoDeCotacaoDeMaterialFechamentoItemVm
                    {
                        NumeroRequisicao = processoItemMaterial.RequisicaoDeCompra.Numero,
                        NumeroItem = processoItemMaterial.RequisicaoDeCompra.NumeroItem,
                        CodigoIva = cotacaoItemMaterial.Iva.Codigo,
                        Selecionada = cotacaoItemMaterial.Selecionada,
                        QuantidadeComprada = cotacaoItemMaterial.QuantidadeAdquirida ?? 0,
                        Preco = cotacaoItemMaterial.Preco
                    }).ToList());

                var vm = new ProcessoDeCotacaoDeMaterialFechamentoComunicacaoSapVm
                    {
                        IdProcessoCotacao = processo.Id,
                        NumeroCotacao = cotacao.NumeroDaCotacao,
                        CodigoFornecedor = fornecedorParticipante.Fornecedor.Codigo,
                        DocumentoDoSap = fechamentoVm.DocumentoParaGerarNoSap,
                        NotaDeCabecalho = fechamentoVm.NotaDeCabecalho,
                        TextoDeCabecalho = fechamentoVm.TextoDeCabecalho,
                        CodigoCondicaoPagamento = cotacao.CondicaoDePagamento.Codigo,
                        CodigoIncoterm1 = cotacao.Incoterm.Codigo,
                        Incoterm2 = cotacao.DescricaoIncoterm,
                        Itens = itens
                    };

                //comentado enquanto o serviço do sap não é implementado
                //_comunicacaoSap.EnviarMensagem("", vm);

            }
        }
    }
}