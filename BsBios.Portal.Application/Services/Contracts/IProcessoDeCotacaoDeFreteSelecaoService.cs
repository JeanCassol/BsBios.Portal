using BsBios.Portal.ViewModel;

namespace BsBios.Portal.Application.Services.Contracts
{
    public interface IProcessoDeCotacaoDeFreteSelecaoService
    {
        void AtualizarSelecao(ProcessoDeCotacaoDeFreteSelecaoAtualizarVm processoDeCotacaoSelecaoAtualizarVm);
    }
}
