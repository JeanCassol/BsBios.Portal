using BsBios.Portal.ViewModel;

namespace BsBios.Portal.Application.Services.Contracts
{
    public interface ICadastroRequisicaoCompra
    {
        void NovaRequisicao(RequisicaoDeCompraVm requisicaoDeCompraVm);
    }
}