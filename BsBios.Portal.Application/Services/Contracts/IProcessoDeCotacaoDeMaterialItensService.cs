using System.Collections.Generic;

namespace BsBios.Portal.Application.Services.Contracts
{
    public interface IProcessoDeCotacaoDeMaterialItensService
    {
        void AtualizarItens(int idProcessoCotacao, IList<int> idsDasRequisicoesDeCompra);

    }
}