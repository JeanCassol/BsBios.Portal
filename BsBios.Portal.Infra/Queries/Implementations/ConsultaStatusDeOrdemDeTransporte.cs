using System;
using System.Collections.Generic;
using System.Linq;
using BsBios.Portal.Common;
using BsBios.Portal.Infra.Queries.Builders;
using BsBios.Portal.Infra.Queries.Contracts;
using BsBios.Portal.ViewModel;

namespace BsBios.Portal.Infra.Queries.Implementations
{
    public class ConsultaStatusDeOrdemDeTransporte : IConsultaStatusDeOrdemDeTransporte
    {

        private readonly IBuilder<Enumeradores.StatusDaOrdemDeTransporte, ChaveValorVm> _builder;

        public ConsultaStatusDeOrdemDeTransporte(IBuilder<Enumeradores.StatusDaOrdemDeTransporte, ChaveValorVm> builder)
        {
            _builder = builder;
        }

        public IList<ChaveValorVm> Listar()
        {
            var status = Enum.GetValues(typeof(Enumeradores.StatusDaOrdemDeTransporte)).Cast<Enumeradores.StatusDaOrdemDeTransporte>().ToList();

            return _builder.BuildList(status);
        }
    }
}