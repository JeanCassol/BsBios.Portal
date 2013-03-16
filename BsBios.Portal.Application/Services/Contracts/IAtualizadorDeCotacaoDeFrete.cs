using BsBios.Portal.ViewModel;

namespace BsBios.Portal.Application.Services.Contracts
{
    public interface IAtualizadorDeCotacaoDeFrete
    {
        void Atualizar(CotacaoInformarVm cotacaoAtualizarVm);
    }
}