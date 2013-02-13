using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BsBios.Portal.Infra.Repositories.Contracts;
using BsBios.Portal.ViewModel;

namespace Application.Queries.Builders
{
    public class ListagemBuilder<TEntidade, TViewModel>
    {
        //public ListagemBuilder(  builder)
        //{
        //}

        public ListagemVm<TViewModel> Build(IQueryable<TEntidade> queryable )
        {
            return new ListagemVm<TViewModel>()
                {
                    TotalDeRegistros = queryable.Count(),
        //            Registros = 
                };
        }
    }
}
