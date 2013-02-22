using BsBios.Portal.Domain.Entities;
using BsBios.Portal.Domain.Services.Contracts;
using BsBios.Portal.ViewModel;

namespace BsBios.Portal.Domain.Services.Implementations
{
    public class CadastroFornecedorOperacao : ICadastroFornecedorOperacao
    {
        public Fornecedor Criar(FornecedorCadastroVm fornecedorCadastroVm)
        {
            var fornecedor = new Fornecedor(fornecedorCadastroVm.Codigo, fornecedorCadastroVm.Nome, fornecedorCadastroVm.Email);
            return fornecedor;
        }

        public void Atualizar(Fornecedor fornecedor, FornecedorCadastroVm novosDados)
        {
            fornecedor.Atualizar(novosDados.Nome, novosDados.Email);
        }
    }
}