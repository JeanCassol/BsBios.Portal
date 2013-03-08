using System.Collections.Generic;
using BsBios.Portal.ViewModel;

namespace BsBios.Portal.Application.Services.Contracts
{
    public interface IProcessoDeCotacaoSelecaoService
    {
        void AtualizarSelecao(ProcessoDeCotacaoSelecaoAtualizarVm processoDeCotacaoSelecaoAtualizarVm);
    }
}
