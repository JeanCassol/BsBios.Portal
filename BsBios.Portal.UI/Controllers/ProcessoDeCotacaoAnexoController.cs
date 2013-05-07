using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BsBios.Portal.Infra.Services.Contracts;
using BsBios.Portal.UI.Filters;
using BsBios.Portal.ViewModel;

namespace BsBios.Portal.UI.Controllers
{
    [SecurityFilter]
    public class ProcessoDeCotacaoAnexoController : Controller
    {
        private readonly IFileService _fileService ;

        public ProcessoDeCotacaoAnexoController(IFileService fileService)
        {
            _fileService = fileService;
        }

        public JsonResult Listar(int idProcessoCotacao)
        {
            string path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Uploads",Convert.ToString(idProcessoCotacao));
            IEnumerable<string> arquivos = _fileService.ListFiles(path);
            var kendoGridVm = new KendoGridVm
                {
                    QuantidadeDeRegistros = arquivos.Count(),
                    Registros = arquivos.Select(f => new FileDownloadVm
                        {
                            FileName = f ,
                            Url = f
                        }).Cast<ListagemVm>().ToList()
                };

            return Json(kendoGridVm, JsonRequestBehavior.AllowGet);
        }

        public FileResult Download(int idProcessoCotacao, string nomeDoArquivo)
        {
            return
                File(
                    Path.Combine(AppDomain.CurrentDomain.BaseDirectory,"Uploads",Convert.ToString(idProcessoCotacao),nomeDoArquivo), 
                    "application/octet-stream", nomeDoArquivo);
            
        }

        [HttpPost]
        public JsonResult Excluir(int idProcessoCotacao, string nomeDoArquivo)
        {
            try
            {
                _fileService.Excluir(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Uploads",
                                                  Convert.ToString(idProcessoCotacao), nomeDoArquivo));
                return Json(new { Sucesso = true, Mensagem = string.Format("Arquivo {0} excluído com sucesso.", nomeDoArquivo) });
            }
            catch (Exception ex)
            {
                return Json(new {Sucesso = false, Mensagem = ex.Message});
            }
        }

    }
}
