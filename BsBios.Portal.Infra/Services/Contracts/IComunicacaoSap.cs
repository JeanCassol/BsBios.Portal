using BsBios.Portal.ViewModel;

namespace BsBios.Portal.Infra.Services.Contracts
{
    public interface IComunicacaoSap<in T>
    {
        ApiResponseMessage EnviarMensagem(string endereco, T mensagem);
    }
}