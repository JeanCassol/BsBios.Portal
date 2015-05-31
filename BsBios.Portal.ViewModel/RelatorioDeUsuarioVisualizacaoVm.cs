using System.Collections.Generic;

namespace BsBios.Portal.ViewModel
{
    public class RelatorioDeUsuarioVisualizacaoVm
    {
        public RelatorioDeUsuarioCabecalhoVm Cabecalho { get; set; }
        public IEnumerable<RelatorioDeUsuarioListagemVm> Usuarios{ get; set; }
    }
}
