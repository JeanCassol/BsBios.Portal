﻿using System.Collections.Generic;
using BsBios.Portal.ViewModel;

namespace BsBios.Portal.Application.Services.Contracts
{
    public interface ICadastroFornecedor
    {
        void Novo(FornecedorCadastroVm fornecedorCadastroVm);
        void AtualizarFornecedores(IList<FornecedorCadastroVm> fornecedores);
    }
}