using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BsBios.Portal.Application.Queries.Builders;
using BsBios.Portal.Application.Queries.Contracts;
using BsBios.Portal.Domain.Entities;
using BsBios.Portal.Infra.Repositories.Contracts;
using BsBios.Portal.ViewModel;

namespace BsBios.Portal.Application.Queries.Implementations
{
    public class ConsultaProduto: IConsultaProduto
    {
        private readonly IProdutos _produtos;
        private readonly IBuilder<Fornecedor, FornecedorCadastroVm> _builder;

        public ConsultaProduto(IProdutos produtos, IBuilder<Fornecedor, FornecedorCadastroVm> builder)
        {
            _produtos = produtos;
            _builder = builder;
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


    }
}
