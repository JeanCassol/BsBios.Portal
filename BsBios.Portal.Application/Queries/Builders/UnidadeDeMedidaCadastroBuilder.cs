using BsBios.Portal.Domain.Entities;
using BsBios.Portal.ViewModel;

namespace BsBios.Portal.Application.Queries.Builders
{
    public class UnidadeDeMedidaCadastroBuilder : Builder<UnidadeDeMedida, UnidadeDeMedidaCadastroVm>
    {
        public override UnidadeDeMedidaCadastroVm BuildSingle(UnidadeDeMedida model)
        {
            return new UnidadeDeMedidaCadastroVm()
                {
                    CodigoInterno = model.CodigoInterno ,
                    CodigoExterno = model.CodigoExterno,
                    Descricao = model.Descricao,
                };
        }

    }
}
