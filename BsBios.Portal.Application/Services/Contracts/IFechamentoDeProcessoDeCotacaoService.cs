using BsBios.Portal.ViewModel;

namespace BsBios.Portal.Application.Services.Contracts
{
    public interface IFechamentoDeProcessoDeCotacaoService
    {
        void Executar(ProcessoDeCotacaoFechamentoVm processoDeCotacaoFechamentoVm);
    }
}