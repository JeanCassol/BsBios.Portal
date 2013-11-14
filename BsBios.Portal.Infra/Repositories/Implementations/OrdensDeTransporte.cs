using System;
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
        public IOrdensDeTransporte BuscaPorId(int id)
        {
            Query = Query.Where(ordem => ordem.Id == id);
            return this;
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