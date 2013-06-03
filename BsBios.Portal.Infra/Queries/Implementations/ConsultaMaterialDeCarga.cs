using System;
using System.Collections.Generic;
using System.Linq;
using BsBios.Portal.Common;
using BsBios.Portal.Infra.Queries.Builders;
using BsBios.Portal.Infra.Queries.Contracts;
using BsBios.Portal.ViewModel;

namespace BsBios.Portal.Infra.Queries.Implementations
{
    public class ConsultaMaterialDeCarga : IConsultaMaterialDeCarga
    {
        private readonly IBuilder<Enumeradores.MaterialDeCarga, MaterialDeCargaVm> _builder;

        public ConsultaMaterialDeCarga(IBuilder<Enumeradores.MaterialDeCarga, MaterialDeCargaVm> builder)
        {
            _builder = builder;
        }

        public IList<MaterialDeCargaVm> Listar()
        {
            var materiaisDeCarga = Enum.GetValues(typeof(Enumeradores.MaterialDeCarga)).Cast<Enumeradores.MaterialDeCarga>().ToList();
            
            return _builder.BuildList(materiaisDeCarga);

        }
    }
}