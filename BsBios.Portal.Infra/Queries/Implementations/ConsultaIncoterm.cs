using System.Collections.Generic;
using BsBios.Portal.Domain.Entities;
using BsBios.Portal.Infra.Queries.Builders;
using BsBios.Portal.Infra.Queries.Contracts;
using BsBios.Portal.Infra.Repositories.Contracts;
using BsBios.Portal.ViewModel;

namespace BsBios.Portal.Infra.Queries.Implementations
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
