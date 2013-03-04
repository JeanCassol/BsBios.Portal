using BsBios.Portal.ViewModel;

namespace BsBios.Portal.Application.Services.Contracts
{
    public interface IAtualizadorDeCotacao
    {
        void Atualizar(CotacaoAtualizarVm cotacaoAtualizarVm);
    }
}