﻿using BsBios.Portal.ViewModel;

namespace BsBios.Portal.Application.Services.Contracts
{
    public interface IProcessoDeCotacaoService
    {
        void AtualizarFornecedores(AtualizacaoDosFornecedoresDoProcessoDeCotacaoVm atualizacaoDosFornecedoresVm);
    }
}