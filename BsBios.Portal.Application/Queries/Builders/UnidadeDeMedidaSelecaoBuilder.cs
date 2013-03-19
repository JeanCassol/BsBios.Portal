using BsBios.Portal.Domain.Entities;
using BsBios.Portal.ViewModel;

namespace BsBios.Portal.Application.Queries.Builders
{
    public class UnidadeDeMedidaSelecaoBuilder : Builder<UnidadeDeMedida, UnidadeDeMedidaSelecaoVm>
    {
        public override UnidadeDeMedidaSelecaoVm BuildSingle(UnidadeDeMedida model)
        {
            return new UnidadeDeMedidaSelecaoVm()
                {
                    CodigoInterno = model.CodigoInterno ,
                    Descricao = model.CodigoExterno + " - " +  model.Descricao,
                };
        }

    }
}
