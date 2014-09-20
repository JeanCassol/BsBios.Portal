﻿using System.Collections.Generic;
using BsBios.Portal.Application.Queries.Builders;
using BsBios.Portal.Application.Queries.Contracts;
using BsBios.Portal.Domain.Entities;
using BsBios.Portal.Domain.Repositories;
using BsBios.Portal.ViewModel;

namespace BsBios.Portal.Application.Queries.Implementations
{
    public class ConsultaIncoterm: IConsultaIncoterm
    {
        private readonly IIncoterms _incoterms;
        private readonly IBuilder<Incoterm, IncotermCadastroVm> _builder;

        public ConsultaIncoterm(IIncoterms incoterms, IBuilder<Incoterm, IncotermCadastroVm> builder)
        {
            _incoterms = incoterms;
            _builder = builder;
        }

        public IList<IncotermCadastroVm> ListarTodos()
        {
            return _builder.BuildList(_incoterms.List());
        }
    }
}
