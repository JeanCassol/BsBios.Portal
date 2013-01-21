using System;
using System.Linq;
using BsBios.Portal.Domain.Model;
using BsBios.Portal.Domain.Services.Contracts;

namespace BsBios.Portal.Domain.Services.Implementations
{
    public class SelecionaFornecedor: ISelecionaFornecedor
    {
        public Fornecedor Selecionar(Material material, DateTime data)
        {
            return material.Fornecedores.FirstOrDefault();
        }
    }
}
