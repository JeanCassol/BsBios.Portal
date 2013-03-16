using BsBios.Portal.ViewModel;

namespace BsBios.Portal.Application.Services.Contracts
{
    public interface IAtualizadorDeCotacaoDeMaterial
    {
        void Atualizar(CotacaoMaterialInformarVm cotacaoAtualizarVm);
    }
}