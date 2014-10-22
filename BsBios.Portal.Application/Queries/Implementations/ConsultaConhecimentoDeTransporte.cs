using System;
using System.Linq;
using BsBios.Portal.Application.DTO;
using BsBios.Portal.Application.Queries.Contracts;
using BsBios.Portal.Common;
using BsBios.Portal.Domain.Entities;
using BsBios.Portal.ViewModel;
using NHibernate;
using NHibernate.Linq;

namespace BsBios.Portal.Application.Queries.Implementations
{
    public class ConsultaConhecimentoDeTransporte : IConsultaConhecimentoDeTransporte
    {

        private readonly ISession _session;

        public ConsultaConhecimentoDeTransporte(ISession session)
        {
            _session = session;
        }

        public KendoGridVm Listar(PaginacaoVm paginacao, FiltroDeConhecimentoDeTransporte filtro)
        {
            IQueryable<ConhecimentoDeTransporte> queryable = _session.Query<ConhecimentoDeTransporte>();

            if (!string.IsNullOrEmpty(filtro.CodigoDoFornecedor))
            {
                queryable = queryable.Where(ct => ct.Fornecedor.Codigo.Contains(filtro.CodigoDoFornecedor));
            }

            if (!string.IsNullOrEmpty(filtro.CodigoDaTransportadora))
            {
                queryable = queryable.Where(ct => ct.Transportadora.Codigo.Contains(filtro.CodigoDaTransportadora));
            }

            if (filtro.DataDeEmissao.HasValue)
            {
                queryable = queryable.Where(ct => ct.DataDeEmissao == filtro.DataDeEmissao);
            }

            if (!string.IsNullOrEmpty(filtro.NumeroDoContrato))
            {
                
            }

            if (filtro.Status.HasValue)
            {
                var statusDoConhecimentoDeTransporte = (Enumeradores.StatusDoConhecimentoDeTransporte) Enum.Parse(typeof(Enumeradores.StatusDoConhecimentoDeTransporte),Convert.ToString(filtro.Status.Value));
                queryable = queryable.Where(ct => ct.Status == statusDoConhecimentoDeTransporte);
            }

            var kendoGridVm = new KendoGridVm
            {
                QuantidadeDeRegistros = queryable.Count(),
                Registros = queryable
                    .OrderByDescending(x => x.DataDeEmissao)
                    .Skip(paginacao.Skip)
                    .Take(paginacao.Take)
                    .Select(ct => new ConhecimentoDeTransporteListagem
                    {
                        ChaveEletronica = ct.ChaveEletronica,
                        DataDeEmissao = ct.DataDeEmissao.ToShortDateString(),
                        CodigoDoFornecedor = ct.Fornecedor.Codigo,
                        CodigoDaTransportadora = ct.Transportadora.Codigo,
                        Status = ct.Status
                    }).Cast<ListagemVm>().ToList(),
            };


            return kendoGridVm;
            

        }
    }
}