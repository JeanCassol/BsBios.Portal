using System.Linq;
using BsBios.Portal.Application.Queries.Builders;
using BsBios.Portal.Application.Queries.Contracts;
using BsBios.Portal.Domain.Entities;
using BsBios.Portal.Domain.Repositories;
using BsBios.Portal.ViewModel;

namespace BsBios.Portal.Application.Queries.Implementations
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
