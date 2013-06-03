using System;
using System.Collections.Generic;
using System.Linq;
using BsBios.Portal.Common;
using BsBios.Portal.Infra.Queries.Builders;
using BsBios.Portal.Infra.Queries.Contracts;
using BsBios.Portal.ViewModel;

namespace BsBios.Portal.Infra.Queries.Implementations
{
    public class ConsultaFluxoDeCarga : IConsultaFluxoDeCarga
    {
        private readonly IBuilder<Enumeradores.FluxoDeCarga, FluxoDeCargaVm> _builder;

        public ConsultaFluxoDeCarga(IBuilder<Enumeradores.FluxoDeCarga, FluxoDeCargaVm> builder)
        {
            _builder = builder;
        }

        public IList<FluxoDeCargaVm> Listar()
        {
            var fluxosDeCarga = Enum.GetValues(typeof(Enumeradores.FluxoDeCarga)).Cast<Enumeradores.FluxoDeCarga>().ToList();
            
            return _builder.BuildList(fluxosDeCarga);

        }
    }
}