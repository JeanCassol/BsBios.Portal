using System.Collections.Generic;
using System.Linq;
using BsBios.Portal.Domain.Entities;
using BsBios.Portal.ViewModel;

namespace BsBios.Portal.Infra.Queries.Builders
{
    public class IvaCadastroBuilder : IBuilder<Iva, IvaCadastroVm>
    {
        public IvaCadastroVm BuildSingle(Iva iva)
        {
            return new IvaCadastroVm()
                {
                    Codigo = iva.Codigo,
                    Descricao = iva.Descricao
                };
        }

        public IList<IvaCadastroVm> BuildList(IList<Iva> ivas)
        {
            return ivas.Select(condicaoDePagamento => new IvaCadastroVm()
                {
                    Codigo = condicaoDePagamento.Codigo, 
                    Descricao = condicaoDePagamento.Descricao
                }).ToList();
        }
    }
}
