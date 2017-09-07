using System.Collections.Generic;
using System.Linq;
using BsBios.Portal.Domain.Entities;
using BsBios.Portal.ViewModel;

namespace BsBios.Portal.Infra.Queries.Builders
{
    public class IncotermCadastroBuilder : IBuilder<Incoterm, IncotermCadastroVm>
    {
        public IncotermCadastroVm BuildSingle(Incoterm incoterm)
        {
            return new IncotermCadastroVm()
            {
                Codigo = incoterm.Codigo,
                Descricao = incoterm.Descricao
            };
        }

        public IList<IncotermCadastroVm> BuildList(IList<Incoterm> incoterms)
        {
            return incoterms.Select(incoterm => new IncotermCadastroVm()
            {
                Codigo = incoterm.Codigo,
                Descricao = incoterm.Descricao
            }).ToList();
        }
    }
}
