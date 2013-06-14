using BsBios.Portal.Domain.Entities;
using BsBios.Portal.ViewModel;

namespace BsBios.Portal.Infra.Services.Contracts
{
    public interface IProcessoDeCotacaoDeMaterialFechamentoComunicacaoSap
    {
        void EfetuarComunicacao(ProcessoDeCotacaoDeMaterial processo, ProcessoDeCotacaoDeMaterialFechamentoInfoVm fechamentoVm);
    }
}