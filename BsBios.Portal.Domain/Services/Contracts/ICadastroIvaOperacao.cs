﻿using BsBios.Portal.Domain.Entities;
using BsBios.Portal.ViewModel;

namespace BsBios.Portal.Domain.Services.Contracts
{
    public interface ICadastroIvaOperacao
    {
        Iva Criar(IvaCadastroVm ivaCadastroVm);
        void Alterar(Iva iva, IvaCadastroVm ivaCadastroVm);
    }
}