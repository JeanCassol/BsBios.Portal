﻿using BsBios.Portal.Domain.Entities;

namespace BsBios.Portal.Infra.Repositories.Contracts
{
    public interface IUsuarios: ICompleteRepository<Usuario>
    {
        Usuario BuscaPorLogin(string login);
        IUsuarios FiltraPorNome(string filtroNome);
        IUsuarios FiltraPorListaDeLogins(string[] logins);
    }
}