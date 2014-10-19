using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using BsBios.Portal.Application.Services.Contracts;
using BsBios.Portal.Common.Exceptions;
using BsBios.Portal.UI.Filters;
using BsBios.Portal.ViewModel;

namespace BsBios.Portal.UI.Controllers
{
    [ApiAuthorizationFilter]
    public class ProdutoFornecedorApiController : ApiController
    {
        private readonly ICadastroProdutoFornecedor _cadastroProdutoFornecedor;

        public ProdutoFornecedorApiController(ICadastroProdutoFornecedor cadastroProdutoFornecedor)
        {
            _cadastroProdutoFornecedor = cadastroProdutoFornecedor;
        }

        public HttpResponseMessage AtualizaFornecedoresDoProduto([FromBody]  ListaProdutoFornecedor listaProdutoFornecedor)
        {
            ApiResponseMessage retornoPortal;

            try
            {
                var registros = (from pf in listaProdutoFornecedor
                                 group pf by pf.CodigoProduto
                                 into grouped
                                 select new {produto = grouped.Key, fornecedores = grouped.Select(x => x.CodigoFornecedor).ToArray()}
                                ).ToList();

                string detalhes = "";
                string codigoRetorno = "200";
                foreach (var registro in registros)
                {
                    try
                    {
                        _cadastroProdutoFornecedor.AtualizarFornecedoresDoProduto(registro.produto, registro.fornecedores);

                        if (!string.IsNullOrEmpty(detalhes))
                        {
                            detalhes += Environment.NewLine;
                        }
                        detalhes += "Produto: " + registro.produto + " - " + registro.fornecedores.Count() + " fornecedores atualizados;";
                    }
                    catch (Exception ex)
                    {
                        if (!string.IsNullOrEmpty(detalhes))
                        {
                            detalhes += Environment.NewLine;
                        }

                        detalhes += "Produto: " + registro.produto + " - erro ao atualizar fornecedores: " + ex.Message + ";";
                        codigoRetorno = "500";
                    }
                }
                
                retornoPortal = new ApiResponseMessage()
                {
                    Retorno = new Retorno() { Codigo = codigoRetorno, Texto = detalhes}
                };
                return Request.CreateResponse(HttpStatusCode.OK, retornoPortal);
            }

            catch (Exception ex)
            {
                retornoPortal = ExceptionUtil.GeraMensagemDeErroParaWebApi(ex); 
                return Request.CreateResponse(HttpStatusCode.OK, retornoPortal);
            }
        }
    }
}