using System.Collections.Generic;
using System.Linq;
using BsBios.Portal.Domain.Entities;
using BsBios.Portal.Domain.Repositories;
using BsBios.Portal.ViewModel;

namespace BsBios.Portal.Infra.Queries.Implementations
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
            return _builder.BuildList(_ivas.GetQuery().OrderBy(x => x.Descricao).ToList());
        }
    }
}
