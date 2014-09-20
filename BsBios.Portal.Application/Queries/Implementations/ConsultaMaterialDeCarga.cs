﻿using System.Collections.Generic;
using BsBios.Portal.Application.Queries.Builders;
using BsBios.Portal.Application.Queries.Contracts;
using BsBios.Portal.Domain.Entities;
using BsBios.Portal.Domain.Repositories;
using BsBios.Portal.ViewModel;

namespace BsBios.Portal.Application.Queries.Implementations
{
    public class ConsultaMaterialDeCarga : IConsultaMaterialDeCarga
    {
        private readonly IBuilder<MaterialDeCarga, MaterialDeCargaVm> _builder;
        private readonly IMateriaisDeCarga _materiasDeCarga;

        public ConsultaMaterialDeCarga(IBuilder<MaterialDeCarga, MaterialDeCargaVm> builder, IMateriaisDeCarga materiasDeCarga)
        {
            _builder = builder;
            _materiasDeCarga = materiasDeCarga;
        }

        public IList<MaterialDeCargaVm> Listar()
        {
            var materiaisDeCarga = _materiasDeCarga.List();
            
            return _builder.BuildList(materiaisDeCarga);

        }
    }
}