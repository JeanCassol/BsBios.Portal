using BsBios.Portal.Domain.Model;
using BsBios.Portal.ViewModel;

namespace BsBios.Portal.Domain.Services.Contracts
{
    public interface ICadastroFornecedorOperacao
    {
        Fornecedor Criar(FornecedorCadastroVm fornecedorCadastroVm);
        void Atualizar(Fornecedor fornecedor, FornecedorCadastroVm novosDados);
    }

}