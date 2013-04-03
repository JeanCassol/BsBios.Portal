using BsBios.Portal.Domain.Entities;
using BsBios.Portal.Infra.Services.Contracts;
using BsBios.Portal.ViewModel;

namespace BsBios.Portal.Infra.Services.Implementations
{
    public class ComunicacaoAberturaProcessoCotacaoFrete : IComunicacaoSap
    {
        public ApiResponseMessage EfetuarComunicacao(ProcessoDeCotacao processo)
        {
            return new ApiResponseMessage
                {
                    Retorno = new Retorno
                        {
                            Codigo = "200",
                            Texto = "S"
                        }
                };
        }
    }
}