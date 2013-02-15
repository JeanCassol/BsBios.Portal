using System;
using System.Net;
using System.Net.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;

namespace BsBios.Portal.UI.Filters
{
    public class ApiAuthorizationFilter: AuthorizationFilterAttribute
    {
        private void SetResponseUsuarioNaoAutorizado(HttpActionContext actionContext)
        {
            actionContext.Response = new HttpResponseMessage(HttpStatusCode.Unauthorized)
            {
                Content = new StringContent("Usuário não autorizado")
            };
            
        }
        public override void OnAuthorization(HttpActionContext actionContext)
        {
            if (actionContext.Request.Headers.Authorization == null)
            {
                SetResponseUsuarioNaoAutorizado(actionContext);
                return;
            }
            string encodedParameter = actionContext.Request.Headers.Authorization.Parameter;
            byte[] decodedParameterArray = Convert.FromBase64String(encodedParameter);
            string decodedParameter = System.Text.Encoding.ASCII.GetString(decodedParameterArray);
            string[] credenciais = decodedParameter.Split(':');
            string usuario = credenciais[0].ToLower();
            string senha = credenciais[1];

            if (usuario != "sap" || senha != "123")
            {
                SetResponseUsuarioNaoAutorizado(actionContext);
            }
        }
    }
}