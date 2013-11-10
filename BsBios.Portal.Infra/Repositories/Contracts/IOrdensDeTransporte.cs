using BsBios.Portal.Domain.Entities;

namespace BsBios.Portal.Infra.Repositories.Contracts
{
    public interface IOrdensDeTransporte: ICompleteRepository<OrdemDeTransporte>
    {
        IOrdensDeTransporte BuscaPorId(int id);
        IOrdensDeTransporte AutorizadasParaOFornecedor(string codigoDoFornecedor);
        IOrdensDeTransporte CodigoDoFornecedorContendo(string codigoDoFornecedor);
        IOrdensDeTransporte NomeDoFornecedorContendo(string nomeDoFornecedor);
    }
}