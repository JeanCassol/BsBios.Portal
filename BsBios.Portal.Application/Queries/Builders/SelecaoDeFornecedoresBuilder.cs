using BsBios.Portal.Application.Queries.Contracts;
using BsBios.Portal.Common;

namespace BsBios.Portal.Application.Queries.Builders
{
    public class SelecaoDeFornecedoresBuilder: Builder<Enumeradores.SelecaoDeFornecedores, SelecaoDeFornecedoresVm>
    {
        public override SelecaoDeFornecedoresVm BuildSingle(Enumeradores.SelecaoDeFornecedores model)
        {
            return new SelecaoDeFornecedoresVm()
                {
                    Codigo = (int) model,
                    Descricao = model.Descricao()
                };
        }
    }
}
