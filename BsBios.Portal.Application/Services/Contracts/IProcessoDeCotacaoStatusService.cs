using BsBios.Portal.Infra.Services.Contracts;

namespace BsBios.Portal.Application.Services.Contracts
{
    public interface IProcessoDeCotacaoStatusService
    {
        IComunicacaoSap ComunicacaoSap { get; set; }
        //void AbrirProcesso(int idProcessoCotacao);
        void FecharProcesso(int idProcessoCotacao);
    }
}