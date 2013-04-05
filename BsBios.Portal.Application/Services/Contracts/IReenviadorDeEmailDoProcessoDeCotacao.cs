namespace BsBios.Portal.Application.Services.Contracts
{
    public interface IReenviadorDeEmailDoProcessoDeCotacao
    {
        void ReenviarEmailDeAbertura(int idProcessoCotacao, int idFornecedorParticipante);
    }
}