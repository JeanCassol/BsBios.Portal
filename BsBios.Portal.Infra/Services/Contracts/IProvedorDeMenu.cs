using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BsBios.Portal.Domain;
using BsBios.Portal.Infra.Model;

namespace BsBios.Portal.Infra.Services.Contracts
{
    public interface IProvedorDeMenu
    {
        IList<Menu> ObtemPorPerfil(Enumeradores.Perfil perfil);
    }
}
