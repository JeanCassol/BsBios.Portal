using System.Collections.Generic;

namespace BsBios.Portal.Application.Services.Contracts
{
    public interface IServicoDeEficienciaDeNegociacao
    {
        string[] ListarFornecedores(string numeroDaRequisicao, string numeroDoItem);
        IList<dynamic> CalcularEficienciaDoItemDoProcesso(string numeroDaRequisicao, string numeroDoItem);
    }
}