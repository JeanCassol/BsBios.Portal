using System.Linq;
using BsBios.Portal.Domain.Entities;
using BsBios.Portal.Infra.Repositories.Contracts;

namespace BsBios.Portal.Infra.Repositories.Implementations
{
    public class OrdensDeTransporte : CompleteRepositoryNh<OrdemDeTransporte>, IOrdensDeTransporte
    {
        public OrdensDeTransporte(IUnitOfWorkNh unitOfWork) : base(unitOfWork)
        {
        }
        public OrdemDeTransporte BuscaPorId(int id)
        {
            return Query.SingleOrDefault(ordem => ordem.Id == id);
        }

        public IOrdensDeTransporte AutorizadasParaOFornecedor(string codigoDoFornecedor)
        {
            Query = Query.Where(x => x.Fornecedor.Codigo == codigoDoFornecedor);
            return this;
        }

        public IOrdensDeTransporte CodigoDoFornecedorContendo(string codigoDoFornecedor)
        {
            if (!string.IsNullOrEmpty(codigoDoFornecedor))
            {
                Query = Query.Where(x => x.Fornecedor.Codigo.ToLower().Contains(codigoDoFornecedor.ToLower()));
            }
            return this;
        }

        public IOrdensDeTransporte NomeDoFornecedorContendo(string nomeDoFornecedor)
        {
            if (!string.IsNullOrEmpty(nomeDoFornecedor))
            {
                Query = Query.Where(x => x.Fornecedor.Nome.ToLower().Contains(nomeDoFornecedor.ToLower()));

            }
            return this;
        }
    }
}