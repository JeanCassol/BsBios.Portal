namespace BsBios.Portal.Infra.Services.Contracts
{
    public interface IGeradorDeSenha
    {
        string Gerar(int numeroDeCaracteresAlfabeticos, int numeroDeCaracteresNumericos);
        string GerarGuid(int tamanho);
    }
}