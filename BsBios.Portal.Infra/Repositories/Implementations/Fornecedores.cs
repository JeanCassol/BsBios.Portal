using System.Linq;
using BsBios.Portal.Domain.Entities;
using BsBios.Portal.Domain.Repositories;

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

        public IFornecedores BuscaPeloCnpj(string cnpj)
        {
            Query = Query.Where(x => x.Cnpj == cnpj);

            return this;

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

        public IFornecedores NomeContendo(string filtroNome)
        {
            if (!string.IsNullOrEmpty(filtroNome))
            {
                Query = Query.Where(x => x.Nome.ToLower().Contains(filtroNome.ToLower()));
                
            }
            return this;
        }

        public IFornecedores CodigoContendo(string filtroCodigo)
        {
            if (!string.IsNullOrEmpty(filtroCodigo))
            {
                Query = Query.Where(x => x.Codigo.ToLower().Contains(filtroCodigo.ToLower()));
            }

            return this;
        }

        public IFornecedores SomenteTransportadoras()
        {
            Query = Query.Where(x => x.Transportadora);
            return this;
        }

        public IFornecedores RemoveTransportadoras()
        {
            Query = Query.Where(x => !x.Transportadora);
            return this;
        }
    }
}
