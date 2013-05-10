using System;

namespace BsBios.Portal.ViewModel
{
    public class RequisicaoDeCompraFiltroVm
    {
        public string CodigoDoGrupoDeCompras { get; set; }
        public DateTime? DataDeSolicitacaoInicial { get; set; }
        public DateTime? DataDeSolicitacaoFinal { get; set; }
    }
}