using BsBios.Portal.Domain.Entities;

namespace BsBios.Portal.Domain.Repositories
{
    public interface IRequisicoesDeCompra:ICompleteRepository<RequisicaoDeCompra>
    {
        RequisicaoDeCompra BuscaPeloId(int id);
    }
}