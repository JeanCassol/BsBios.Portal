using BsBios.Portal.ViewModel;

namespace BsBios.Portal.Application.Services.Contracts
{
    public interface IProcessoDeCotacaoDeMaterialSelecaoService
    {
        void AtualizarSelecao(ProcessoDeCotacaoDeMaterialSelecaoAtualizarVm processoDeCotacaoSelecaoAtualizarVm);
    }
}
