using System;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web;
using System.IO;
using BsBios.Portal.Infra.Services.Contracts;

namespace BsBios.Portal.UI.Controllers
{
    public class UploadController : ApiController
    {
        private readonly IFileService _fileService;

        public UploadController(IFileService fileService)
        {
            _fileService = fileService;
        }

        // Enable both Get and Post so that our jquery call can send data, and get a status
        [HttpGet]
        [HttpPost]
        public HttpResponseMessage Upload()
        {
            bool ocorreuErro = false;
            string mensagensDeErro = "";
            try
            {
                string usuario="";
                var windowsIdentity = System.Security.Principal.WindowsIdentity.GetCurrent();
                if (windowsIdentity != null)
                {
                    usuario = windowsIdentity.Name;
                }
                string idProcessoCotacao = HttpContext.Current.Request.Form["IdProcessoCotacao"];
                //foreach (string nomeDoArquivo in HttpContext.Current.Request.Files)
                for (int i = 0; i < HttpContext.Current.Request.Files.Count ; i++)
                {
                    try
                    {
                        HttpPostedFile file = HttpContext.Current.Request.Files[i];
                        string filePath = "";
                        try
                        {
                            filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Uploads");
                            _fileService.Save(filePath,
                                idProcessoCotacao, file.FileName, file.InputStream);

                        }
                        catch (Exception ex)
                        {
                            ocorreuErro = true;
                            if (!string.IsNullOrEmpty(mensagensDeErro))
                            {
                                mensagensDeErro += Environment.NewLine;
                            }
                            mensagensDeErro += "Erro salvando arquivo - Caminho: " + filePath + "Usuário: " + usuario + Environment.NewLine   + ex.Message;
                        }
                    }
                    catch (Exception ex)
                    {
                        ocorreuErro = true;
                        if (!string.IsNullOrEmpty(mensagensDeErro))
                        {
                            mensagensDeErro += Environment.NewLine;
                        }
                        mensagensDeErro += "Erro lendo arquivo: " +  ex.Message;
                    }
                }

                // Now we need to wire up a response so that the calling script understands what happened
                HttpContext.Current.Response.ContentType = "text/plain";

                //HttpContext.Current.Response.Write(serializer.Serialize(mensagensDeErro));
                HttpContext.Current.Response.Write(mensagensDeErro);
                HttpContext.Current.Response.StatusCode = ocorreuErro ? (int)HttpStatusCode.InternalServerError : (int)HttpStatusCode.OK;

                // For compatibility with IE's "done" event we need to return a result as well as setting the context.response
                return new HttpResponseMessage(ocorreuErro? HttpStatusCode.InternalServerError : HttpStatusCode.OK );
            }

            catch (Exception ex)
            {
                HttpContext.Current.Response.Write("Erro tratado: " +  ex.Message);
                HttpContext.Current.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

                // For compatibility with IE's "done" event we need to return a result as well as setting the context.response
                return new HttpResponseMessage(HttpStatusCode.InternalServerError);
            }

        }
    }
}
