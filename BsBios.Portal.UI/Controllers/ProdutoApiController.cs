using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using BsBios.Portal.ApplicationServices.Contracts;
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
        public HttpResponseMessage Post([FromBody]ProdutoCadastroVm produtoCadastroVm)
        {
            try
            {
                _cadastroProduto.Novo(produtoCadastroVm);
                return Request.CreateResponse(HttpStatusCode.OK);

            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex);
            }
        }

        //para funcionar o binding de um xml para um array ou list a classe correspondente ao parâmetro 
        //deve ser decorada com a propriedade "[DataContract]" e as propriedades da classe que precisam
        //ser serializadas devem ser decoradas com a propriedade "[DataMember]"
        //Se na origem da requisição o dado for um json isto não é necessário.
        //Ver explicação em: http://www.asp.net/web-api/overview/formats-and-model-binding/json-and-xml-serialization
        public HttpResponseMessage PostMultiplo([FromBody] IList<ProdutoCadastroVm> produtos)
        {
            try
            {
                _cadastroProduto.AtualizarProdutos(produtos);
                return Request.CreateResponse(HttpStatusCode.OK);
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex);
            }
        }
    }
}