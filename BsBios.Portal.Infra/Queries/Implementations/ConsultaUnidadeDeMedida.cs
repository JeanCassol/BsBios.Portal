using System.Collections.Generic;
using System.Linq;
using BsBios.Portal.Domain.Entities;
using BsBios.Portal.Domain.Repositories;
using BsBios.Portal.Infra.Queries.Builders;
using BsBios.Portal.Infra.Queries.Contracts;
using BsBios.Portal.ViewModel;

namespace BsBios.Portal.Infra.Queries.Implementations
{
    public class ConsultaUnidadeDeMedida : IConsultaUnidadeDeMedida
    {
        private readonly IUnidadesDeMedida _ivas;
        private readonly IBuilder<UnidadeDeMedida, UnidadeDeMedidaSelecaoVm> _builder;

        public ConsultaUnidadeDeMedida(IUnidadesDeMedida unidadesDeMedida, IBuilder<UnidadeDeMedida, UnidadeDeMedidaSelecaoVm> builder)
        {
            _ivas = unidadesDeMedida;
            _builder = builder;
        }

        public IList<UnidadeDeMedidaSelecaoVm> ListarTodos()
        {
            return _builder.BuildList(_ivas.GetQuery().OrderBy(x => x.CodigoExterno).ToList());
        }
    }
}
