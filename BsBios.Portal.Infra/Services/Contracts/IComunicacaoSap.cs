using System;
using BsBios.Portal.ViewModel;

namespace BsBios.Portal.Infra.Services.Contracts
{
    public interface IComunicacaoSap
    {
        ApiResponseMessage EnviarMensagem(string endereco, Object mensagem);
    }
}