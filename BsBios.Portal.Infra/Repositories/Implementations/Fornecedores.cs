using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BsBios.Portal.Domain.Entities;
using BsBios.Portal.Infra.Repositories.Contracts;

namespace BsBios.Portal.Infra.Repositories.Implementations
{
    public class Fornecedores:CompleteRepositoryNh<Fornecedor>,  IFornecedores
    {
        public Fornecedores(IUnitOfWorkNh unitOfWork) : base(unitOfWork)
        {
        }

        public Fornecedor BuscaPeloCodigo(string codigoSap)
        {
            return Query.SingleOrDefault(x => x.Codigo == codigoSap);
        }

        public IFornecedores BuscaListaPorCodigo(string[] codigoDosFornecedores)
        {
            Query = Query.Where(x => codigoDosFornecedores.Contains(x.Codigo));
            return this;
        }

        public IFornecedores FornecedoresNaoVinculadosAoProduto(string codigoProduto)
        {
            Query = Query.Where(fornecedor => fornecedor.Produtos.All(produto => produto.Codigo != codigoProduto));
            return this;
        }
    }
}
