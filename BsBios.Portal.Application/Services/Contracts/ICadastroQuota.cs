using System;
using System.Collections.Generic;
using BsBios.Portal.ViewModel;

namespace BsBios.Portal.Application.Services.Contracts
{
    public interface ICadastroQuota
    {
        void Salvar(DateTime data, IList<QuotaSalvarVm> quotasCadastroVm);
    }
}