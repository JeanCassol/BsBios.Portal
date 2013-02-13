using System.Collections.Generic;
using BsBios.Portal.ViewModel;

namespace Application.Queries.Contracts
{
    public interface IConsultaCondicaoPagamento
    {
        IList<CondicaoDePagamentoCadastroVm> Listar(PaginacaoVm paginacaoVm, CondicaoDePagamentoCadastroVm filtro);
    }
}