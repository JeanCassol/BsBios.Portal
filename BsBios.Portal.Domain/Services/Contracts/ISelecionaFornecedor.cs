using System;
using BsBios.Portal.Domain.Entities;

namespace BsBios.Portal.Domain.Services.Contracts
{
    public interface ISelecionaFornecedor
    {
        Fornecedor Selecionar(Material material, DateTime data);
    }
}