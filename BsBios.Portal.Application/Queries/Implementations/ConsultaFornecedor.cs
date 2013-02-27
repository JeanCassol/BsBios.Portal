using System.Linq;
using BsBios.Portal.Application.Queries.Contracts;
using BsBios.Portal.Domain.Entities;
using BsBios.Portal.Infra.Repositories.Contracts;
using BsBios.Portal.ViewModel;

namespace BsBios.Portal.Application.Queries.Implementations
{
    public class ConsultaFornecedor: IConsultaFornecedor
    {
        private readonly IProdutos _produtos;

        public ConsultaFornecedor(IProdutos produtos)
        {
            _produtos = produtos;
        }

        public KendoGridVm FornecedoresDoProduto(string codigoProduto)
        {
            Produto produto = _produtos.BuscaPeloCodigo(codigoProduto);
            var kendoGrid = new KendoGridVm()
                {
                    QuantidadeDeRegistros = produto.Fornecedores.Count,
                    Registros = produto.Fornecedores.Select(x => new FornecedorCadastroVm()
                        {
                            Codigo = x.Codigo ,
                            Nome = x.Nome,
                            Email = x.Email
                        }).Cast<ListagemVm>().ToList()
                };

            return kendoGrid;
        }
    }
}
