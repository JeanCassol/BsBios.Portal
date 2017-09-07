using System.Collections.Generic;
using BsBios.Portal.ViewModel;

namespace BsBios.Portal.Infra.Queries.Contracts
{
    public interface IConsultaDeRelatorioDeUsuario
    {
        IEnumerable<RelatorioDeUsuarioListagemVm> Listar(RelatorioDeUsuarioFiltroVm filtro);

    }
}