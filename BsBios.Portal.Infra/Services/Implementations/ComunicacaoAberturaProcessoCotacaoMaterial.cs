﻿using System;
using System.Linq;
using BsBios.Portal.Common;
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
            //seleciona os participantes que preencheram a cotação (cotacaoMaterial != null)
            //e que ainda não possuem número da cotação. O esperado é que nenhuma das cotações possuem NumeroDaCotacao.
            //Isto acontecerá apenas se ocorrer erro na comunicação com o sap em para uns dos fornecedores do processo
            //e antes deste fornecedor, a comunicação tenha sido realizada com sucesso para outro fornecedor. 
            //Neste cenário o fornecedor anterior já tem um número de cotação e não deve ser mais enviado para o SAP,
            //pois isto faria com que fosse criado duas cotações para o mesmo fornecedor e item de requisição, o que é errado.
            var participantesSemCotacaoNoSap = (from fp in processo.FornecedoresParticipantes
                                                let cotacaoMaterial = (CotacaoMaterial) fp.Cotacao.CastEntity()
                                                where string.IsNullOrEmpty(fp.NumeroDaCotacao)
                                                select fp);
            
            foreach (var fornecedorParticipante in participantesSemCotacaoNoSap )
            {
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
                        DataLimiteRetorno = processo.DataLimiteDeRetorno.Value.ToString("dd.MM.yyyy"),
                        Itens = itens
                    };

                //comentado enquanto o serviço do SAP não é implementado
                ApiResponseMessage resposta = _comunicacaoSap.EnviarMensagem("/HttpAdapter/HttpMessageServlet?senderParty=PORTAL&senderService=HTTP&interfaceNamespace=http://portal.bsbios.com.br/&interface=si_cadCotacaoAbertura_portal&qos=be", vm);
                if (resposta.Retorno.Codigo.Equals("200") && !string.IsNullOrEmpty(resposta.Retorno.Texto))
                {
                     fornecedorParticipante.AtualizarNumeroDaCotacao(resposta.Retorno.Texto);
                }
                else
                {
                    throw new Exception("Ocorreu um erro ao enviar os dados do Processo de Cotação para o SAP.");
                }

            }
            return new ApiResponseMessage(){Retorno = new Retorno(){Codigo = "200" ,Texto = "OK" }};
        }
    }
}