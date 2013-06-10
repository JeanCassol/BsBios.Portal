using BsBios.Portal.Domain.Entities;

namespace BsBios.Portal.Infra.Repositories.Contracts
{
    public interface IProcessosDeCotacaoDeMaterial: IProcessosDeCotacao
    {
        //IProcessosDeCotacao GeradosPelaRequisicaoDeCompra(string numeroDaRequisicao, string numeroDoItem);
        IProcessosDeCotacao GeradosPelaRequisicaoDeCompra(int idRequisicaoDeCompra);
    }
}