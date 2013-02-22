using BsBios.Portal.Domain.Entities;

namespace BsBios.Portal.Infra.Repositories.Contracts
{
    public interface IRequisicoesDeCompra:ICompleteRepository<RequisicaoDeCompra>
    {
        RequisicaoDeCompra BuscaPeloId(int id);
    }
}