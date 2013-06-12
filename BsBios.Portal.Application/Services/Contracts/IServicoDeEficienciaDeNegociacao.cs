using System.Collections.Generic;

namespace BsBios.Portal.Application.Services.Contracts
{
    public interface IServicoDeEficienciaDeNegociacao
    {
        IList<dynamic> CalcularEficienciaDoItemDoProcesso(int idProcessoCotacao, int idProcessoCotacaoItem);
    }
}