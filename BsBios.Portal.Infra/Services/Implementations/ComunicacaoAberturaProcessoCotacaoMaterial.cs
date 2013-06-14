using System;
using System.Linq;
using BsBios.Portal.Domain.Entities;
using BsBios.Portal.Infra.Services.Contracts;
using BsBios.Portal.ViewModel;

namespace BsBios.Portal.Infra.Services.Implementations
{
    public class ComunicacaoAberturaProcessoCotacaoMaterial : IProcessoDeCotacaoComunicacaoSap
    {
        private readonly IComunicacaoSap<ProcessoDeCotacaoDeMaterialAberturaComunicacaoSapVm> _comunicacaoSap;

        public ComunicacaoAberturaProcessoCotacaoMaterial(IComunicacaoSap<ProcessoDeCotacaoDeMaterialAberturaComunicacaoSapVm> comunicacaoSap)
        {
            _comunicacaoSap = comunicacaoSap;
        }

        public ApiResponseMessage EfetuarComunicacao(ProcessoDeCotacao processo)
        {
            foreach (var fornecedorParticipante in processo.FornecedoresParticipantes)
            {
                var cotacaoMaterial = (CotacaoMaterial)fornecedorParticipante.Cotacao;
                if (cotacaoMaterial != null && !string.IsNullOrEmpty(cotacaoMaterial.NumeroDaCotacao))
                {
                    continue;
                }

                var itens = new ListaProcessoDeCotacaoDeMaterialAberturaItemVm();
                itens.AddRange(from ProcessoDeCotacaoDeMaterialItem item in processo.Itens
                               select new ProcessoDeCotacaoDeMaterialAberturaItemVm
                                   {
                                       NumeroRequisicao = item.RequisicaoDeCompra.Numero, 
                                       NumeroItem = item.RequisicaoDeCompra.NumeroItem
                                   });

                var vm = new ProcessoDeCotacaoDeMaterialAberturaComunicacaoSapVm
                    {
                        IdProcessoCotacao = processo.Id,
                        CodigoFornecedor = fornecedorParticipante.Fornecedor.Codigo ,
                        Itens = itens
                    };

                ApiResponseMessage resposta =  _comunicacaoSap.EnviarMensagem("", vm);
                if (resposta.Retorno.Codigo.Equals("200"))
                {
                    if (cotacaoMaterial != null)
                    {
                        cotacaoMaterial.AtualizarNumeroDaCotacao(resposta.Retorno.Texto);
                    }
                }
                else
                {
                    throw new Exception(resposta.Retorno.Texto);
                }

            }
            return new ApiResponseMessage(){Retorno = new Retorno(){Codigo = "200" ,Texto = "OK" }};
        }
    }
}