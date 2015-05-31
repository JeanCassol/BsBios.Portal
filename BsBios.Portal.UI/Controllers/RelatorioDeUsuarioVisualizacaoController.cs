using System;
using System.Collections.Generic;
using System.Web.Mvc;
using BsBios.Portal.Application.Queries.Contracts;
using BsBios.Portal.Common;
using BsBios.Portal.UI.Filters;
using BsBios.Portal.ViewModel;

namespace BsBios.Portal.UI.Controllers
{
    [SecurityFilter]
    public class RelatorioDeUsuarioVisualizacaoController : BaseController
    {
        private readonly IConsultaDeRelatorioDeUsuario _consulta;

        public RelatorioDeUsuarioVisualizacaoController(IConsultaDeRelatorioDeUsuario consulta)
        {
            _consulta = consulta;
        }

        [HttpPost]
        public ActionResult Relatorio(RelatorioDeUsuarioFiltroVm filtro)
        {
            IEnumerable<RelatorioDeUsuarioListagemVm> usuarios = _consulta.Listar(filtro);

            var visualizacaoVm = new RelatorioDeUsuarioVisualizacaoVm
            {
                Cabecalho = ConstruirCabecalho(filtro),
                Usuarios = usuarios
            };

            return View(visualizacaoVm);

        }

        private RelatorioDeUsuarioCabecalhoVm ConstruirCabecalho(RelatorioDeUsuarioFiltroVm filtro)
        {
            string descricaoDoFiltroDeStatus = filtro.Status == 0 
                ? "Todos" 
                : ((Enumeradores.StatusUsuario) Enum.Parse(typeof (Enumeradores.StatusUsuario), Convert.ToString(filtro.Status))).Descricao();

            return new RelatorioDeUsuarioCabecalhoVm
            {
                Login = string.IsNullOrEmpty(filtro.Login) ? "Todos": filtro.Login,
                Nome = string.IsNullOrEmpty(filtro.Nome) ? "Todos": filtro.Nome,
                Email = string.IsNullOrEmpty(filtro.Email) ? "Todos": filtro.Email,
                Status = descricaoDoFiltroDeStatus
            };
        }
    }
}