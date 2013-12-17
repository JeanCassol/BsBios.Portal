﻿using System;
using System.Collections.Generic;
using System.Linq;
using BsBios.Portal.Application.Queries.Builders;
using BsBios.Portal.Application.Queries.Contracts;
using BsBios.Portal.Common;
using BsBios.Portal.ViewModel;

namespace BsBios.Portal.Application.Queries.Implementations
{
    public class ConsultaEscolhaSimples : IConsultaEscolhaSimples
    {

        private readonly IBuilder<Enumeradores.EscolhaSimples, ChaveValorVm> _builder;

        public ConsultaEscolhaSimples(IBuilder<Enumeradores.EscolhaSimples, ChaveValorVm> builder)
        {
            _builder = builder;
        }

        public IList<ChaveValorVm> Listar()
        {
            var status = Enum.GetValues(typeof(Enumeradores.EscolhaSimples)).Cast<Enumeradores.EscolhaSimples>().ToList();

            return _builder.BuildList(status);

        }
    }
}