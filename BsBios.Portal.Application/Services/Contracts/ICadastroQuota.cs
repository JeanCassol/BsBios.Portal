using BsBios.Portal.ViewModel;

namespace BsBios.Portal.Application.Services.Contracts
{
    public interface ICadastroQuota
    {
        void Salvar(QuotasSalvarVm quotasSalarVm);
    }
}