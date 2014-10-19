using System;
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
    public class ItinerarioApiController : ApiController
    {
        private readonly ICadastroItinerario _cadastroItinerario;

        public ItinerarioApiController(ICadastroItinerario cadastroItinerario)
        {
            _cadastroItinerario = cadastroItinerario;
        }

        //para funcionar o binding de um xml para um array ou list a classe correspondente ao parâmetro 
        //deve ser decorada com a propriedade "[DataContract]" e as propriedades da classe que precisam
        //ser serializadas devem ser decoradas com a propriedade "[DataMember]"
        //Se na origem da requisição o dado for um json isto não é necessário.
        //Ver explicação em: http://www.asp.net/web-api/overview/formats-and-model-binding/json-and-xml-serialization
        public HttpResponseMessage PostMultiplo([FromBody] ListaItinerarios itinerarios)
        {
            ApiResponseMessage retornoPortal;
            try
            {
                _cadastroItinerario.AtualizarItinerarios(itinerarios);
                retornoPortal = new ApiResponseMessage()
                    {
                        Retorno = new Retorno() {Codigo = "200", Texto = itinerarios.Count + " itinerários atualizados"}
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