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
        private readonly IBuilder<Fornecedor, FornecedorCadastroVm> _builder;

        public ConsultaFornecedor(IFornecedores fornecedores, IBuilder<Fornecedor, FornecedorCadastroVm> builder)
        {
            _builder = builder;
            _fornecedores = fornecedores;
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

        public KendoGridVm Listar(PaginacaoVm paginacaoVm, string filtroNome)
        {
            if (!string.IsNullOrEmpty(filtroNome))
            {
                _fornecedores.FiltraPorNome(filtroNome);
            }
            var kendoGridVmn = new KendoGridVm()
                {
                    QuantidadeDeRegistros = _fornecedores.Count(),
                    Registros =
                        _builder.BuildList(_fornecedores.Skip(paginacaoVm.Skip).Take(paginacaoVm.Take).List())
                                .Cast<ListagemVm>()
                                .ToList()

                };
            return kendoGridVmn;
        }

        public FornecedorCadastroVm ConsultaPorCodigo(string codigoDoFornecedor)
        {
            return _builder.BuildSingle(_fornecedores.BuscaPeloCodigo(codigoDoFornecedor));
        }

        public KendoGridVm ProdutosDoFornecedor(string codigoFornecedor)
        {
            throw new System.NotImplementedException();
        }
    }
}
