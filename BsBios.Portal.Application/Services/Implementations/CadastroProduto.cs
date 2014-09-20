using System;
using System.Collections.Generic;
using System.Linq;
using BsBios.Portal.Application.Services.Contracts;
using BsBios.Portal.Domain.Entities;
using BsBios.Portal.Domain.Repositories;
using BsBios.Portal.ViewModel;

namespace BsBios.Portal.Application.Services.Implementations
{
    public class CadastroProduto: ICadastroProduto
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IProdutos _produtos;
        private IList<Produto> _produtosConsultados;

        public CadastroProduto(IUnitOfWork unitOfWork, IProdutos produtos)
        {
            _unitOfWork = unitOfWork;
            _produtos = produtos;
        }

        private void AtualizarProduto(ProdutoCadastroVm produtoCadastroVm)
        {
            Produto produto = _produtosConsultados.SingleOrDefault(x => x.Codigo == produtoCadastroVm.Codigo);
            if (produto != null)
            {
                produto.Atualizar(produtoCadastroVm.Descricao, produtoCadastroVm.Tipo);
            }
            else
            {
                produto = new Produto(produtoCadastroVm.Codigo, produtoCadastroVm.Descricao, produtoCadastroVm.Tipo);
            }
            _produtos.Save(produto);
        }

        public void Novo(ProdutoCadastroVm produtoCadastroVm)
        {
            try
            {
                _unitOfWork.BeginTransaction();
                _produtosConsultados = _produtos.FiltraPorListaDeCodigos(new[] {produtoCadastroVm.Codigo}).List();
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
                _produtosConsultados = _produtos.FiltraPorListaDeCodigos(produtos.Select(x => x.Codigo).ToArray()).List();
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
