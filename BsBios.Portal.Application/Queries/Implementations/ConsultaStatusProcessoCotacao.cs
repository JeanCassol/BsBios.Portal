using System;
using System.Collections.Generic;
using System.Linq;
using BsBios.Portal.Application.Queries.Builders;
using BsBios.Portal.Application.Queries.Contracts;
using BsBios.Portal.Common;
using BsBios.Portal.ViewModel;

namespace BsBios.Portal.Application.Queries.Implementations
{
    public class ConsultaStatusProcessoCotacao : IConsultaStatusProcessoCotacao
    {
        private readonly IBuilder<Enumeradores.StatusProcessoCotacao, StatusProcessoCotacaoVm> _builder;

        public ConsultaStatusProcessoCotacao(IBuilder<Enumeradores.StatusProcessoCotacao, StatusProcessoCotacaoVm> builder)
        {
            _builder = builder;
        }

        public IList<StatusProcessoCotacaoVm> Listar()
        {
            var status = Enum.GetValues(typeof(Enumeradores.StatusProcessoCotacao)).Cast<Enumeradores.StatusProcessoCotacao>().ToList();
            
            return _builder.BuildList(status);
        }
    }
}