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
    public class ConhecimentoDeTransporteApiController : ApiController
    {
        private readonly ICadastroDeConhecimentoDeTransporte _cadastroDeConhecimentoDeTransporte;

        public ConhecimentoDeTransporteApiController(ICadastroDeConhecimentoDeTransporte cadastroDeConhecimentoDeTransporte)
        {
            _cadastroDeConhecimentoDeTransporte = cadastroDeConhecimentoDeTransporte;
        }

        public HttpResponseMessage PostMultiplo([FromBody] ListaDeConhecimentoDeTransporte conhecimentosDeTransporte)
        {
            ApiResponseMessage retornoPortal;
            try
            {
                _cadastroDeConhecimentoDeTransporte.Salvar(conhecimentosDeTransporte);
                retornoPortal = new ApiResponseMessage()
                    {
                        Retorno = new Retorno() { Codigo = "200", Texto = string.Format("{0} Conhecimentos de Transporte recebidos", conhecimentosDeTransporte.Count) }
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