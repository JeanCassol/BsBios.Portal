using BsBios.Portal.Common;
using BsBios.Portal.Infra.Queries.Contracts;

namespace BsBios.Portal.Infra.Queries.Builders
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
