using System;
using System.Collections.Generic;
using System.Linq;
using BsBios.Portal.Application.Queries.Builders;
using BsBios.Portal.Application.Queries.Contracts;
using BsBios.Portal.Common;
using BsBios.Portal.ViewModel;

namespace BsBios.Portal.Application.Queries.Implementations
{
    public class ConsultaPerfil : IConsultaPerfil
    {
        private readonly IBuilder<Enumeradores.Perfil, PerfilVm> _builder;

        public ConsultaPerfil(IBuilder<Enumeradores.Perfil, PerfilVm> builder)
        {
            _builder = builder;
        }

        public IList<PerfilVm> Listar()
        {
            var perfis = Enum.GetValues(typeof(Enumeradores.Perfil)).Cast<Enumeradores.Perfil>().ToList();
            
            return _builder.BuildList(perfis);
        }
    }
}