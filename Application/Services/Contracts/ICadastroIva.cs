using BsBios.Portal.ViewModel;

namespace BsBios.Portal.ApplicationServices.Contracts
{
    public interface ICadastroIva
    {
        void Novo(IvaCadastroVm ivaCadastroVm);
    }
}