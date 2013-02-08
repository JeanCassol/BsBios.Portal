using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BsBios.Portal.ApplicationServices.Contracts;
using BsBios.Portal.Domain.Model;
using BsBios.Portal.Infra.Repositories.Contracts;
using BsBios.Portal.ViewModel;

namespace BsBios.Portal.ApplicationServices.Implementation
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
            Produto produto = _produtos.BuscaPorCodigoSap(produtoCadastroVm.CodigoSap);
            if (produto != null)
            {
                produto.AtualizaDescricao(produtoCadastroVm.Descricao);
            }
            else
            {
                produto = new Produto(produtoCadastroVm.CodigoSap, produtoCadastroVm.Descricao);
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
            }
            catch (Exception)
            {
                _unitOfWork.RollBack();
                throw;
            }
        }
    }
}
