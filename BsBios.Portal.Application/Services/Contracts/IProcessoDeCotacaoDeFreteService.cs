using BsBios.Portal.ViewModel;

namespace BsBios.Portal.Application.Services.Contracts
{
    public interface IProcessoDeCotacaoDeFreteService
    {
        void Salvar(ProcessoCotacaoFreteCadastroVm processoCotacaoFreteCadastroVm);
    }
}