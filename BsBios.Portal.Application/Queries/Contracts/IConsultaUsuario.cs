using System.Collections.Generic;
using BsBios.Portal.ViewModel;

namespace BsBios.Portal.Application.Queries.Contracts
{
    public interface IConsultaUsuario
    {
        KendoGridVm Listar(PaginacaoVm paginacaoVm, UsuarioFiltroVm usuarioFiltroVm);
        UsuarioConsultaVm ConsultaPorLogin(string login);
        IList<PerfilVm> PerfisDoUsuario(string login);
    }
}