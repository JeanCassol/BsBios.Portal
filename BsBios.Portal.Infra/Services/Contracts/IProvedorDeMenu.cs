using System.Collections.Generic;
using BsBios.Portal.Common;
using BsBios.Portal.Infra.Model;

namespace BsBios.Portal.Infra.Services.Contracts
{
    public interface IProvedorDeMenu
    {
        IList<Menu> ObtemPorPerfil(Enumeradores.Perfil perfil);
    }
}
