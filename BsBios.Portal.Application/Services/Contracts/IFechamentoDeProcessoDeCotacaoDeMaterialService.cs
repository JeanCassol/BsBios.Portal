using BsBios.Portal.ViewModel;

namespace BsBios.Portal.Application.Services.Contracts
{
    public interface IFechamentoDeProcessoDeCotacaoDeMaterialService
    {
        void Executar(ProcessoDeCotacaoDeMaterialFechamentoInfoVm processoDeCotacaoFechamentoVm);
    }
}