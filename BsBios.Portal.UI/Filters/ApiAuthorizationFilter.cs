using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http.Filters;

namespace BsBios.Portal.UI.Filters
{
    public class ApiAuthorizationFilter: AuthorizationFilterAttribute
    {
        public override void OnAuthorization(System.Web.Http.Controllers.HttpActionContext actionContext)
        {
            string encodedParameter = actionContext.Request.Headers.Authorization.Parameter;
            byte[] decodedParameterArray = Convert.FromBase64String(encodedParameter);
            string decodedParameter = System.Text.Encoding.ASCII.GetString(decodedParameterArray);
            string[] credenciais = decodedParameter.Split(':');
            string usuario = credenciais[0].ToLower();
            string senha = credenciais[1];

            if (usuario != "sap" || senha != "123")
            {
                actionContext.Response = new HttpResponseMessage(HttpStatusCode.Unauthorized)
                    {
                        Content = new StringContent("Usuário não autorizado")
                    };
            }
        }
    }
}