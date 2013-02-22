using System.Collections.Generic;
using System.Linq;
using BsBios.Portal.Domain.Entities;
using BsBios.Portal.ViewModel;

namespace BsBios.Portal.Application.Queries.Builders
{
    public class CondicaoPagamentoCadastroBuilder
    {
        //public CondicaoDePagamentoCadastroVm BuildSingle(CondicaoDePagamento condicaoDePagamento)
        //{
        //    return new CondicaoDePagamentoCadastroVm()
        //        {
        //            Id = condicaoDePagamento.Id,
        //            CodigoSap = condicaoDePagamento.CodigoSap,
        //            Descricao = condicaoDePagamento.Descricao
        //        };
        //}

        public IList<CondicaoDePagamentoCadastroVm> BuildList(IList<CondicaoDePagamento> condicoesDePagamento)
        {
            return condicoesDePagamento.Select(condicaoDePagamento => new CondicaoDePagamentoCadastroVm()
                {
                    Codigo = condicaoDePagamento.Codigo, 
                    Descricao = condicaoDePagamento.Descricao
                }).ToList();
        }
    }
}
