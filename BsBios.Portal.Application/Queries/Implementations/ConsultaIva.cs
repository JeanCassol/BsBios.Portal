using System.Collections.Generic;
using BsBios.Portal.Application.Queries.Builders;
using BsBios.Portal.Application.Queries.Contracts;
using BsBios.Portal.Domain.Entities;
using BsBios.Portal.Infra.Repositories.Contracts;
using BsBios.Portal.ViewModel;

namespace BsBios.Portal.Application.Queries.Implementations
{
    public class ConsultaIva : IConsultaIva
    {
        private readonly IIvas _ivas;
        private readonly IBuilder<Iva, IvaCadastroVm> _builder;

        public ConsultaIva(IIvas ivas, IBuilder<Iva, IvaCadastroVm> builder)
        {
            _ivas = ivas;
            _builder = builder;
        }

        public IList<IvaCadastroVm> ListarTodos()
        {
            return _builder.BuildList(_ivas.List());
        }
    }
}
