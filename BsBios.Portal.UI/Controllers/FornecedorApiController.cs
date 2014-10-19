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
        public HttpResponseMessage PostMultiplo([FromBody] ListaFornecedores fornecedores)
        {
            ApiResponseMessage retornoPortal;
            try
            {
                var fornecedoresComNome = fornecedores.Where(x => !string.IsNullOrEmpty(x.Nome)).ToList();
                int quantidadeDeFornecedoresSemNome = fornecedores.Count - fornecedoresComNome.Count;
                _cadastroFornecedor.AtualizarFornecedores(fornecedoresComNome);
                retornoPortal = new ApiResponseMessage()
                    {
                        Retorno = new Retorno() {Codigo = "200", Texto = fornecedoresComNome.Count + " fornecedores atualizados." +
                        (quantidadeDeFornecedoresSemNome > 0 ? quantidadeDeFornecedoresSemNome +  " fornecedores não atualizados: "  + 
                        string.Join(", ",fornecedores.Where(x => string.IsNullOrEmpty(x.Nome)).Select(f => f.Codigo)) + ".": "") }
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