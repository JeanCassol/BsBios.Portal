using System;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using BsBios.Portal.Application.Services.Contracts;
using BsBios.Portal.UI.Filters;
using BsBios.Portal.ViewModel;

namespace BsBios.Portal.UI.Controllers
{
    [ApiAuthorizationFilter]
    public class ProdutoApiController : ApiController
    {
        private readonly ICadastroProduto _cadastroProduto;

        public ProdutoApiController(ICadastroProduto cadastroProduto)
        {
            _cadastroProduto = cadastroProduto;
        }

        // POST api/<controller>
        public HttpResponseMessage Post([FromBody] ProdutoCadastroVm produtoCadastroVm)
        {
            try
            {
                _cadastroProduto.Novo(produtoCadastroVm);
                return Request.CreateResponse(HttpStatusCode.OK);
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        //para funcionar o binding de um xml para um array ou list a classe correspondente ao parâmetro 
        //deve ser decorada com a propriedade "[DataContract]" e as propriedades da classe que precisam
        //ser serializadas devem ser decoradas com a propriedade "[DataMember]"
        //Se na origem da requisição o dado for um json isto não é necessário.
        //Ver explicação em: http://www.asp.net/web-api/overview/formats-and-model-binding/json-and-xml-serialization
        public HttpResponseMessage PostMultiplo([FromBody] /*IList<ProdutoCadastroVm>*/ ListaProdutos produtos)
        {
            ApiResponseMessage retornoPortal;
            try
            {
                _cadastroProduto.AtualizarProdutos(produtos);
                retornoPortal = new ApiResponseMessage()
                    {
                        Retorno = new Retorno() {Codigo = "200", Texto = produtos.Count + " produtos atualizados"}
                        //retorno = new retorno() { retCodigo = "200", retTexto =    produtos.Produtos.Count + " produtos atualizados" }
                    };
                return Request.CreateResponse(HttpStatusCode.OK, retornoPortal);
            }

            catch (Exception ex)
            {

                string detalhes = "";
                if (produtos == null)
                {
                    detalhes = "A lista de produtos esta nula";
                }
                else if (produtos.Count == 0)
                {
                    detalhes = "A lista de produtos esta vazia";
                }
                else
                {
                    for (int i = 0; i < produtos.Count; i++)
                    {
                        detalhes += "Produto " + i + ": Codigo: " + ( string.IsNullOrEmpty(produtos[i].CodigoSap)
                                                ? "nulo" : produtos[i].CodigoSap)
                                                  + " - Tipo: " + (string.IsNullOrEmpty(produtos[i].Tipo)
                                                  ? "nulo" : produtos[i].Tipo
                                                  + " - Descricao: "  +  (string.IsNullOrEmpty(produtos[i].Descricao)
                                                  ? "nulo": produtos[i].Descricao));
                    }
                }

                retornoPortal = new ApiResponseMessage()
                    {
                        Retorno = new Retorno() {Codigo = "500", Texto = "Erro interno. Mensagem: " + ex.Message 
                            + ( ex.InnerException != null ? " - Excecao Interna: " + ex.InnerException.Message : "")
                            + " - Pilha de Execucao: " + ex.StackTrace + " - Detalhes: " + detalhes  }

                    };
                return Request.CreateResponse(HttpStatusCode.OK, retornoPortal);
            }
        }

        //[HttpGet]
        public ListaProdutos GetProdutos()
        {
            return new ListaProdutos()
                {
                    new ProdutoCadastroVm()
                        {
                            CodigoSap = "SAP1000",
                            Descricao = "Bio Diesel",
                            Tipo = "1"
                        },
                    new ProdutoCadastroVm()
                        {
                            CodigoSap = "SAP2000",
                            Descricao = "Soja",
                            Tipo = "2"
                        }
                };
        }
    }
}