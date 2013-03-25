using BsBios.Portal.ViewModel;

namespace BsBios.Portal.Application.Services.Contracts
{
    public interface IAgendamentoDeCargaService
    {
        QuotaPesoVm SalvarAgendamentoDeCarregamento(AgendamentoDeCarregamentoCadastroVm agendamentoDeCarregamentoCadastroVm);
        QuotaPesoVm SalvarAgendamentoDeDescarregamento(AgendamentoDeDescarregamentoCadastroVm agendamentoDeCarregamentoCadastroVm);
        QuotaPesoVm ExcluirAgendamento(int idQuota, int idAgendamento);
    }
}