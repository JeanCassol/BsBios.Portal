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

        public void Novo(ProdutoCadastroVm produtoCadastroVm)
        {
            try
            {
                _unitOfWork.BeginTransaction();
                var produto = new Produto(produtoCadastroVm.CodigoSap, produtoCadastroVm.Descricao);
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
