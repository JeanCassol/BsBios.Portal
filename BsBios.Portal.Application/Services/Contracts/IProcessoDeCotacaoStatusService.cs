namespace BsBios.Portal.Application.Services.Contracts
{
    public interface IProcessoDeCotacaoStatusService
    {
        void AbrirProcesso(int idProcessoCotacao);
        void FecharProcesso(int idProcessoCotacao);
    }
}