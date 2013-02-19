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
    public class FornecedorApiController : ApiController
    {
        private readonly ICadastroFornecedor _cadastroFornecedor;

        public FornecedorApiController(ICadastroFornecedor cadastroFornecedor)
        {
            _cadastroFornecedor = cadastroFornecedor;
        }

        //para funcionar o binding de um xml para um array ou list a classe correspondente ao parâmetro 
        //deve ser decorada com a propriedade "[DataContract]" e as propriedades da classe que precisam
        //ser serializadas devem ser decoradas com a propriedade "[DataMember]"
        //Se na origem da requisição o dado for um json isto não é necessário.
        //Ver explicação em: http://www.asp.net/web-api/overview/formats-and-model-binding/json-and-xml-serialization
        public HttpResponseMessage PostMultiplo([FromBody] /*IList<ProdutoCadastroVm>*/ ListaFornecedores fornecedores)
        {
            ApiResponseMessage retornoPortal;
            try
            {
                _cadastroFornecedor.AtualizarFornecedores(fornecedores);
                retornoPortal = new ApiResponseMessage()
                    {
                        Retorno = new Retorno() {Codigo = "200", Texto = fornecedores.Count + " fornecedores atualizados"}
                    };
                return Request.CreateResponse(HttpStatusCode.OK, retornoPortal);
            }

            catch (Exception ex)
            {
                retornoPortal = new ApiResponseMessage()
                    {
                        Retorno = new Retorno() {Codigo = "500", Texto = "Erro interno. Mensagem: " + ex.Message 
                            + ( ex.InnerException != null ? " - Excecao Interna: " + ex.InnerException.Message : "")
                            + " - Pilha de Execucao: " + ex.StackTrace}

                    };
                return Request.CreateResponse(HttpStatusCode.OK, retornoPortal);
            }
        }
    }
}