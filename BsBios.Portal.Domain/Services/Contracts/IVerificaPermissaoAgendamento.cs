using BsBios.Portal.Domain.Entities;

namespace BsBios.Portal.Domain.Services.Contracts
{
    public  interface IVerificaPermissaoAgendamento
    {
        bool PermiteEditar(AgendamentoDeCarga agendamentoDeCarga, Usuario usuario);
        bool PermiteRealizar(AgendamentoDeCarga agendamentoDeCarga, Usuario usuario);
        bool PermiteAdicionar(Quota quota, Usuario usuario);
    }
}