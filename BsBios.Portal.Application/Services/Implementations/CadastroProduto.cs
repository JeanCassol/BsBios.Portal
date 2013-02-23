using System;
using System.Collections.Generic;
using BsBios.Portal.Application.Services.Contracts;
using BsBios.Portal.Domain.Entities;
using BsBios.Portal.Infra.Repositories.Contracts;
using BsBios.Portal.ViewModel;

namespace BsBios.Portal.Application.Services.Implementations
{
    public class CadastroProduto: ICadastroProduto
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IProdutos _produtos;

        public CadastroProduto(IUnitOfWork unitOfWork, IProdutos produtos)
        {
            _unitOfWork = unitOfWork;
            _produtos = produtos;
        }

        private void AtualizarProduto(ProdutoCadastroVm produtoCadastroVm)
        {
            Produto produto = _produtos.BuscaPeloCodigo(produtoCadastroVm.CodigoSap);
            if (produto != null)
            {
                produto.Atualizar(produtoCadastroVm.Descricao, produtoCadastroVm.Tipo);
            }
            else
            {
                produto = new Produto(produtoCadastroVm.CodigoSap, produtoCadastroVm.Descricao, produtoCadastroVm.Tipo);
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
