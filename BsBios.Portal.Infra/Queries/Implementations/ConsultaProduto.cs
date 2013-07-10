using System.Collections.Generic;
using System.Linq;
using BsBios.Portal.Domain.Entities;
using BsBios.Portal.Infra.Queries.Builders;
using BsBios.Portal.Infra.Queries.Contracts;
using BsBios.Portal.Infra.Repositories.Contracts;
using BsBios.Portal.ViewModel;

namespace BsBios.Portal.Infra.Queries.Implementations
{
    public class ConsultaProduto: IConsultaProduto
    {
        private readonly IProdutos _produtos;
        private readonly IBuilder<Fornecedor, FornecedorCadastroVm> _builderFornecedor;
        private readonly IBuilder<Produto, ProdutoCadastroVm> _builderProduto;

        public ConsultaProduto(IProdutos produtos, IBuilder<Fornecedor, FornecedorCadastroVm> builder, IBuilder<Produto, ProdutoCadastroVm> builderProduto)
        {
            _produtos = produtos;
            _builderFornecedor = builder;
            _builderProduto = builderProduto;
        }

        public KendoGridVm FornecedoresDoProduto(PaginacaoVm paginacaoVm, string codigoProduto)
        {
            Produto produto = _produtos.BuscaPeloCodigo(codigoProduto);
            if (produto == null)
            {
                return new KendoGridVm
                {
                    Registros = new List<ListagemVm>()
                };
            }

            var kendoGrid = new KendoGridVm
            {
                QuantidadeDeRegistros = produto.Fornecedores.Count,
                Registros = _builderFornecedor.BuildList(
                produto.Fornecedores
                .Skip(paginacaoVm.Skip)
                .Take(paginacaoVm.Take)
                .ToList())
                .Cast<ListagemVm>().ToList()
            };

            return kendoGrid;
        }

        public KendoGridVm FornecedoresDosProdutos(PaginacaoVm paginacaoVm, string[] codigoDosProdutos)
        {
            _produtos.FiltraPorListaDeCodigos(codigoDosProdutos);
            var query = (from p in _produtos.GetQuery()
                                from f in p.Fornecedores
                                                  select f).Distinct();

            return new KendoGridVm
                {
                    QuantidadeDeRegistros = query.Count(),
                    Registros = _builderFornecedor.BuildList(query.Skip(paginacaoVm.Skip).Take(paginacaoVm.Take).ToList()).Cast<ListagemVm>().ToList()
                };
        }

        public ProdutoCadastroVm ConsultaPorCodigo(string codigoProduto)
        {
            return _builderProduto.BuildSingle(_produtos.BuscaPeloCodigo(codigoProduto));
        }

        public KendoGridVm Listar(PaginacaoVm paginacaoVm, ProdutoCadastroVm filtro)
        {
            _produtos.CodigoContendo(filtro.Codigo);
            _produtos.DescricaoContendo(filtro.Descricao);
            _produtos.TipoContendo(filtro.Tipo);

            var kendoGridVmn = new KendoGridVm()
            {
                QuantidadeDeRegistros = _produtos.Count(),
                Registros =
                    _builderProduto.BuildList(_produtos.Skip(paginacaoVm.Skip).Take(paginacaoVm.Take).List())
                            .Cast<ListagemVm>()
                            .ToList()

            };
            return kendoGridVmn;
        }
    }
}
