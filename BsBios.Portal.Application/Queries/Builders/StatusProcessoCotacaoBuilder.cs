using BsBios.Portal.Common;
using BsBios.Portal.ViewModel;

namespace BsBios.Portal.Application.Queries.Builders
{
    public class StatusProcessoCotacaoBuilder: Builder<Enumeradores.StatusProcessoCotacao, StatusProcessoCotacaoVm>
    {
        public override StatusProcessoCotacaoVm BuildSingle(Enumeradores.StatusProcessoCotacao model)
        {
            return new StatusProcessoCotacaoVm()
                {
                    Codigo = (int) model,
                    Descricao = model.Descricao()
                };
        }
    }
}
