using BsBios.Portal.ViewModel;

namespace BsBios.Portal.Application.Services.Contracts
{
    public interface IAtualizadorDeCotacaoDeMaterial
    {
        void AtualizarCotacao(CotacaoMaterialInformarVm cotacaoAtualizarVm);
        void AtualizarItemDaCotacao(CotacaoMaterialItemInformarVm cotacaoMaterialItemInformarVm);
    }
}