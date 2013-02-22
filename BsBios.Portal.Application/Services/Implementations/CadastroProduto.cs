using System;
using System.Collections.Generic;
using System.Linq;
using BsBios.Portal.Application.Services.Contracts;
using BsBios.Portal.Domain.Entities;
using BsBios.Portal.Domain.Services.Contracts;
using BsBios.Portal.Infra.Repositories.Contracts;
using BsBios.Portal.ViewModel;

namespace BsBios.Portal.Application.Services.Implementations
{
    public class CadastroProduto: ICadastroProduto
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IProdutos _produtos;
        private readonly ICadastroProdutoOperacao _atualizadorProduto;

        public CadastroProduto(IUnitOfWork unitOfWork, IProdutos produtos, ICadastroProdutoOperacao atualizadorProduto)
        {
            _unitOfWork = unitOfWork;
            _produtos = produtos;
            _atualizadorProduto = atualizadorProduto;
        }

        private void AtualizarProduto(ProdutoCadastroVm produtoCadastroVm)
        {
            Produto produto = _produtos.BuscaPeloCodigo(produtoCadastroVm.CodigoSap);
            if (produto != null)
            {
                //produto.AtualizaDescricao(produtoCadastroVm.Descricao);
                _atualizadorProduto.Atualizar(produto,produtoCadastroVm);
            }
            else
            {
                //produto = new Produto(produtoCadastroVm.CodigoSap, produtoCadastroVm.Descricao);
                produto = _atualizadorProduto.Criar(produtoCadastroVm);
            }

            _produtos.Save(produto);
        }

        public void Novo(ProdutoCadastroVm produtoCadastroVm)
        {
            try
            {
                _unitOfWork.BeginTransaction();
                AtualizarProduto(produtoCadastroVm);
                _unitOfWork.Commit();
            }
            catch (Exception)
            {
                _unitOfWork.RollBack();
                throw;
            }
        }

        public void AtualizarProdutos(IList<ProdutoCadastroVm> produtos)
        {
            try
            {
                _unitOfWork.BeginTransaction();
                foreach (var produtoCadastroVm in produtos)
                {
                    AtualizarProduto(produtoCadastroVm);
                }
                _unitOfWork.Commit();
                //return produtosAtualizados;
            }
            catch (Exception)
            {
                _unitOfWork.RollBack();
                throw;
            }
        }
    }
}
