using System;
using BsBios.Portal.Domain.Model;

namespace BsBios.Portal.Domain.Services.Contracts
{
    public interface ISelecionaFornecedor
    {
        Fornecedor Selecionar(Material material, DateTime data);
    }
}