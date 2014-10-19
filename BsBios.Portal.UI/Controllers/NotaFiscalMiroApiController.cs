using System;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using BsBios.Portal.Application.Services.Contracts;
using BsBios.Portal.Common.Exceptions;
using BsBios.Portal.ViewModel;

namespace BsBios.Portal.UI.Controllers
{
    public class NotaFiscalMiroApiController : ApiController
    {
        private readonly ICadastroDeNotaFiscalMiro _cadastroDeNotaFiscalMiro;

        public NotaFiscalMiroApiController(ICadastroDeNotaFiscalMiro cadastroDeNotaFiscalMiro)
        {
            _cadastroDeNotaFiscalMiro = cadastroDeNotaFiscalMiro;
        }

        public HttpResponseMessage PostMultiplo([FromBody] ListaDeNotaFiscalMiro notasFiscais)
        {
            ApiResponseMessage retornoPortal;
            try
            {
                _cadastroDeNotaFiscalMiro.Salvar(notasFiscais);
                retornoPortal = new ApiResponseMessage()
                {
                    Retorno = new Retorno() { Codigo = "200", Texto = string.Format("{0} Notas Fiscais recebidas", notasFiscais.Count) }
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
