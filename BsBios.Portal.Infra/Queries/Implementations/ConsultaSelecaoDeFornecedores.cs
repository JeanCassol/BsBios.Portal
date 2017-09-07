using System;
using System.Collections.Generic;
using System.Linq;
using BsBios.Portal.Common;
using BsBios.Portal.Infra.Queries.Builders;
using BsBios.Portal.Infra.Queries.Contracts;

namespace BsBios.Portal.Infra.Queries.Implementations
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