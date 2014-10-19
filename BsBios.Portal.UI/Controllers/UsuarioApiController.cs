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
    public class UsuarioApiController : ApiController
    {
        private readonly ICadastroUsuario _cadastroUsuario;

        public UsuarioApiController(ICadastroUsuario cadastroUsuario)
        {
            _cadastroUsuario = cadastroUsuario;
        }

        //para funcionar o binding de um xml para um array ou list a classe correspondente ao parâmetro 
        //deve ser decorada com a propriedade "[DataContract]" e as propriedades da classe que precisam
        //ser serializadas devem ser decoradas com a propriedade "[DataMember]"
        //Se na origem da requisição o dado for um json isto não é necessário.
        //Ver explicação em: http://www.asp.net/web-api/overview/formats-and-model-binding/json-and-xml-serialization
        public HttpResponseMessage PostMultiplo([FromBody] ListaUsuario usuarios)
        {
            ApiResponseMessage retornoPortal;
            try
            {
                var usuariosComNome = usuarios.Where(x => !string.IsNullOrEmpty(x.Nome)).ToList();
                int quantidadeDeUsuariosSemNome = usuarios.Count - usuariosComNome.Count;
                _cadastroUsuario.AtualizarUsuarios(usuariosComNome);
                retornoPortal = new ApiResponseMessage()
                    {
                        Retorno = new Retorno() {Codigo = "200", Texto = usuariosComNome.Count + " usuários atualizados." + 
                        (quantidadeDeUsuariosSemNome > 0 ? quantidadeDeUsuariosSemNome + " usuários não atualizados: " +
                        string.Join(", ", usuarios.Where(x => string.IsNullOrEmpty(x.Nome)).Select(u => u.Login)) + "." : "")
                        }
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