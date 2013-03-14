using System.Collections.Generic;
using BsBios.Portal.Application.Queries.Builders;
using BsBios.Portal.Application.Queries.Contracts;
using BsBios.Portal.Domain.Entities;
using BsBios.Portal.Infra.Repositories.Contracts;
using BsBios.Portal.ViewModel;
using System.Linq;

namespace BsBios.Portal.Application.Queries.Implementations
{
    public class ConsultaUnidadeDeMedida : IConsultaUnidadeDeMedida
    {
        private readonly IUnidadesDeMedida _ivas;
        private readonly IBuilder<UnidadeDeMedida, UnidadeDeMedidaCadastroVm> _builder;

        public ConsultaUnidadeDeMedida(IUnidadesDeMedida unidadesDeMedida, IBuilder<UnidadeDeMedida, UnidadeDeMedidaCadastroVm> builder)
        {
            _ivas = unidadesDeMedida;
            _builder = builder;
        }

        public IList<UnidadeDeMedidaCadastroVm> ListarTodos()
        {
            return _builder.BuildList(_ivas.GetQuery().OrderBy(x => x.Descricao).ToList());
        }
    }
}
