using System;
using System.Collections.Generic;
using System.Linq;
using BsBios.Portal.Application.Queries.Builders;
using BsBios.Portal.Application.Queries.Contracts;
using BsBios.Portal.Common;

namespace BsBios.Portal.Application.Queries.Implementations
{
    public class ConsultaSelecaoDeFornecedores : IConsultaSelecaoDeFornecedores
    {
        private readonly IBuilder<Enumeradores.SelecaoDeFornecedores, SelecaoDeFornecedoresVm> _builder;

        public ConsultaSelecaoDeFornecedores(IBuilder<Enumeradores.SelecaoDeFornecedores, SelecaoDeFornecedoresVm> builder)
        {
            _builder = builder;
        }

        public IList<SelecaoDeFornecedoresVm> Listar()
        {
            var status = Enum.GetValues(typeof(Enumeradores.SelecaoDeFornecedores)).Cast<Enumeradores.SelecaoDeFornecedores>().ToList();
            
            return _builder.BuildList(status);
        }
    }
}