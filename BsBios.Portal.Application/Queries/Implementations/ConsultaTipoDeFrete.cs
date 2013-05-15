using System;
using System.Collections.Generic;
using System.Linq;
using BsBios.Portal.Application.Queries.Builders;
using BsBios.Portal.Application.Queries.Contracts;
using BsBios.Portal.Common;
using BsBios.Portal.ViewModel;

namespace BsBios.Portal.Application.Queries.Implementations
{
    public class ConsultaTipoDeFrete : IConsultaTipoDeFrete
    {
        private readonly IBuilder<Enumeradores.TipoDeFrete, TipoDeFreteVm> _builder;

        public ConsultaTipoDeFrete(IBuilder<Enumeradores.TipoDeFrete, TipoDeFreteVm> builder)
        {
            _builder = builder;
        }

        public IList<TipoDeFreteVm> Listar()
        {
            var fluxosDeCarga = Enum.GetValues(typeof(Enumeradores.TipoDeFrete)).Cast<Enumeradores.TipoDeFrete>().ToList();
            
            return _builder.BuildList(fluxosDeCarga);
        }
    }
}