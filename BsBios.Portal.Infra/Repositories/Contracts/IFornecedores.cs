﻿using BsBios.Portal.Domain.Model;

namespace BsBios.Portal.Infra.Repositories.Contracts
{
    public interface IFornecedores: ICompleteRepository<Fornecedor>
    {
        Fornecedor BuscaPeloCodigoSap(string codigoSap);
    }
}