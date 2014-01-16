using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Web.Mvc;
using BsBios.Portal.Application.Queries.Contracts;
using BsBios.Portal.Common;
using BsBios.Portal.ViewModel;

namespace BsBios.Portal.UI.Controllers
{
    public class RelatorioParaDownloadController : BaseController
    {

        private readonly IConsultaRelatorioDeProcessoDeCotacaoDeFrete _consulta;

        public RelatorioParaDownloadController(IConsultaRelatorioDeProcessoDeCotacaoDeFrete consulta)
        {
            _consulta = consulta;
        }



    }
}
