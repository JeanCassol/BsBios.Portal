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
        private readonly IFornecedores _fornecedores;
        private readonly IBuilder<Fornecedor, FornecedorCadastroVm> _builderFornecedor;
        private readonly IBuilder<Produto, ProdutoCadastroVm> _builderProduto;

        public ConsultaFornecedor(IFornecedores fornecedores, IBuilder<Fornecedor, FornecedorCadastroVm> builderFornecedor, IBuilder<Produto, ProdutoCadastroVm> builderProduto)
        {
            _builderFornecedor = builderFornecedor;
            _builderProduto = builderProduto;
            _fornecedores = fornecedores;
        }

        public KendoGridVm FornecedoresNaoVinculadosAoProduto(PaginacaoVm paginacaoVm, FornecedorDoProdutoFiltro filtro)
        {
            _fornecedores.FornecedoresNaoVinculadosAosProdutos(filtro.CodigoDosProdutos)
                         .NomeContendo(filtro.NomeFornecedor)
                         .CodigoContendo(filtro.CodigoFornecedor);

            if ( filtro.Transportadora.HasValue && filtro.Transportadora.Value)
            {
                _fornecedores.SomenteTransportadoras();
            }
            else if (filtro.Transportadora.HasValue && ! filtro.Transportadora.Value)
            {
                _fornecedores.RemoveTransportadoras();
            }

            var kendoGrid = new KendoGridVm()
            {
                QuantidadeDeRegistros = _fornecedores.Count(),
                Registros = _builderFornecedor.BuildList(_fornecedores.Skip(paginacaoVm.Skip).Take(paginacaoVm.Take).List()).Cast<ListagemVm>().ToList()
            };

            return kendoGrid;

        }

        public KendoGridVm Listar(PaginacaoVm paginacaoVm, FornecedorFiltroVm filtro)
        {
            _fornecedores
                .CodigoContendo(filtro.Codigo)
                .NomeContendo(filtro.Nome);
            var kendoGridVmn = new KendoGridVm()
                {
                    QuantidadeDeRegistros = _fornecedores.Count(),
                    Registros =
                        _builderFornecedor.BuildList(_fornecedores.Skip(paginacaoVm.Skip).Take(paginacaoVm.Take).List())
                                .Cast<ListagemVm>()
                                .ToList()

                };
            return kendoGridVmn;
        }

        public FornecedorCadastroVm ConsultaPorCodigo(string codigoDoFornecedor)
        {
            return _builderFornecedor.BuildSingle(_fornecedores.BuscaPeloCodigo(codigoDoFornecedor));
        }

        public KendoGridVm ProdutosDoFornecedor(PaginacaoVm paginacaoVm, string codigoFornecedor)
        {
            Fornecedor fornecedor = _fornecedores.BuscaPeloCodigo(codigoFornecedor);
            var kendoGridVm = new KendoGridVm
            {
                QuantidadeDeRegistros = fornecedor.Produtos.Count,
                Registros = _builderProduto.BuildList(
                fornecedor.Produtos
                .Skip(paginacaoVm.Skip)
                .Take(paginacaoVm.Take).ToList())
                .Cast<ListagemVm>().ToList()
            };
            return kendoGridVm;
        }

        public string ConsultaPorCnpj(string cnpj)
        {
            _fornecedores.BuscaPeloCnpj(cnpj);
            return (from fornecedor in _fornecedores.GetQuery() select fornecedor.Nome).FirstOrDefault();
        }
    }
}
