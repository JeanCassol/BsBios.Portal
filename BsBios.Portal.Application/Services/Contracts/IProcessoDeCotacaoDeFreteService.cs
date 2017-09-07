using BsBios.Portal.ViewModel;

namespace BsBios.Portal.Application.Services.Contracts
{
    public interface IProcessoDeCotacaoDeFreteService
    {
        int Salvar(ProcessoCotacaoFreteCadastroVm processoCotacaoFreteCadastroVm);
    }
}