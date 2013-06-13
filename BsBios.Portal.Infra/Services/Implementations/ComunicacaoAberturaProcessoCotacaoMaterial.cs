using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Xml.Serialization;
using BsBios.Portal.Common;
using BsBios.Portal.Common.Exceptions;
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
                var vm = new ProcessoDeCotacaoDeMaterialAberturaComunicacaoSapVm
                    {
                        IdProcessoCotacao = processo.Id,
                        CodigoFornecedor = fornecedorParticipante.Fornecedor.Codigo ,
                        Itens = (ListaProcessoDeCotacaoDeMaterialAberturaItemVm) (from item in processo.Itens
                                 let itemMaterial = (ProcessoDeCotacaoDeMaterialItem) item
                                 select new ProcessoDeCotacaoDeMaterialAberturaItemVm
                                     {
                                         NumeroRequisicao = itemMaterial.RequisicaoDeCompra.Numero ,
                                         NumeroItem = itemMaterial.RequisicaoDeCompra.NumeroItem
                                     }).ToList()
                    };

                ApiResponseMessage resposta =  _comunicacaoSap.EnviarMensagem("", vm);

            }
            return new ApiResponseMessage(){Retorno = new Retorno(){Codigo = "200" ,Texto = "Esta comunicação ainda não foi implementada." }};
        }
    }
}