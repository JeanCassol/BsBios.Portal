using System;
using System.Collections.Generic;
using BsBios.Portal.Application.Services.Contracts;
using BsBios.Portal.Domain.Model;
using BsBios.Portal.Infra.Repositories.Contracts;
using System.Linq;

namespace BsBios.Portal.Application.Services.Implementations
{
    public class CadastroProdutoFornecedor : ICadastroProdutoFornecedor
    {
        private readonly IProdutos _produtos;
        private readonly IFornecedores _fornecedores;
        private readonly IUnitOfWork _unitOfWork;

        public CadastroProdutoFornecedor(IProdutos produtos, IFornecedores fornecedores, IUnitOfWork unitOfWork)
        {
            _produtos = produtos;
            _fornecedores = fornecedores;
            _unitOfWork = unitOfWork;
        }

        public void AtualizarFornecedoresDoProduto(string codigoProduto, string[] codigoDosFornecedores)
        {
            try
            {
                _unitOfWork.BeginTransaction();

                Produto produto = _produtos.BuscaPeloCodigo(codigoProduto);
                //para carregar apenas os fornecedores que ainda não estão associados ao produto, remove do array os que já estão associados
                string[] codigoDosFornecedoresParaCarregar = codigoDosFornecedores.Except(produto.Fornecedores.Select(x => x.Codigo)).ToArray();
                IList<Fornecedor> fornecedoresParaAdicionar = _fornecedores.BuscaListaPorCodigo(codigoDosFornecedoresParaCarregar).List();
                produto.AdicionarFornecedores(fornecedoresParaAdicionar);
                _produtos.Save(produto);

                _unitOfWork.Commit();
            }
            catch (Exception)
            {
                _unitOfWork.RollBack();
                throw;
            }
        }
    }
}