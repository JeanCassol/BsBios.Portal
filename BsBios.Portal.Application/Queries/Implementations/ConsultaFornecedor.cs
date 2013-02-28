using System.Linq;
using BsBios.Portal.Application.Queries.Builders;
using BsBios.Portal.Application.Queries.Contracts;
using BsBios.Portal.Domain.Entities;
using BsBios.Portal.Infra.Repositories.Contracts;
using BsBios.Portal.ViewModel;

namespace BsBios.Portal.Application.Queries.Implementations
{
    public class ConsultaFornecedor: IConsultaFornecedor
    {
        private readonly IProdutos _produtos;
        private readonly IFornecedores _fornecedores;
        private readonly IBuilder<Fornecedor, FornecedorCadastroVm> _builder;

        public ConsultaFornecedor(IProdutos produtos, IFornecedores fornecedores, IBuilder<Fornecedor, FornecedorCadastroVm> builder)
        {
            _produtos = produtos;
            _builder = builder;
            _fornecedores = fornecedores;
        }

        public KendoGridVm FornecedoresDoProduto(string codigoProduto)
        {
            Produto produto = _produtos.BuscaPeloCodigo(codigoProduto);
            var kendoGrid = new KendoGridVm()
                {
                    QuantidadeDeRegistros = produto.Fornecedores.Count,
                    Registros = _builder.BuildList(produto.Fornecedores).Cast<ListagemVm>().ToList()
                };

            return kendoGrid;
        }

        public KendoGridVm FornecedoresNaoVinculadosAoProduto(string codigoProduto)
        {
            _fornecedores.FornecedoresNaoVinculadosAoProduto(codigoProduto);

            var kendoGrid = new KendoGridVm()
            {
                QuantidadeDeRegistros = _fornecedores.Count(),
                Registros = _builder.BuildList(_fornecedores.List()).Cast<ListagemVm>().ToList()
            };

            return kendoGrid;

        }
    }
}
