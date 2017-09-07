using System;
using System.Collections.Generic;
using System.Linq;
using BsBios.Portal.Common;
using BsBios.Portal.Infra.Queries.Builders;
using BsBios.Portal.Infra.Queries.Contracts;
using BsBios.Portal.ViewModel;

namespace BsBios.Portal.Infra.Queries.Implementations
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