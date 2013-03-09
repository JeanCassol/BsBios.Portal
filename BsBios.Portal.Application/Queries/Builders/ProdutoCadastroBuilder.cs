using BsBios.Portal.Domain.Entities;
using BsBios.Portal.ViewModel;

namespace BsBios.Portal.Application.Queries.Builders
{
    public class ProdutoCadastroBuilder: Builder<Produto,ProdutoCadastroVm>
    {
        public override ProdutoCadastroVm BuildSingle(Produto model)
        {
            return new ProdutoCadastroVm()
                {
                    Codigo = model.Codigo ,
                    Descricao = model.Descricao,
                    Tipo = model.Tipo
                };
        }

    }
}
