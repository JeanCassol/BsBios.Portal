using System;

namespace BsBios.Portal.ViewModel
{
    public class RequisicaoDeCompraFiltroVm
    {
        public int IdProcessoCotacao { get; set; }
        public string CodigoDoGrupoDeCompras { get; set; }
        public DateTime? DataDeSolicitacaoInicial { get; set; }
        public DateTime? DataDeSolicitacaoFinal { get; set; }

        public RequisicaoDeCompraFiltroVm(int idProcessoCotacao)
        {
            IdProcessoCotacao = idProcessoCotacao;
        }

        public RequisicaoDeCompraFiltroVm(){}
    }
}