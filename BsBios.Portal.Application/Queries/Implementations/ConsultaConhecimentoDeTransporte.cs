using System;
using System.Linq;
using System.Linq.Expressions;
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

        private Expression<Func<ConhecimentoDeTransporte, ConhecimentoDeTransporteListagem>> GerarProjecao()
        {
            return ct => new ConhecimentoDeTransporteListagem
            {
                ChaveEletronica = ct.ChaveEletronica,
                DataDeEmissao = ct.DataDeEmissao.ToShortDateString(),
                CodigoDoFornecedor = ct.Fornecedor.Codigo,
                CodigoDaTransportadora = ct.Transportadora.Codigo,
                NumeroDoConhecimento = ct.Numero,
                NumeroDoContrato = ct.NumeroDoContrato,
                ValorRealDoFrete = ct.ValorRealDoFrete,
                PesoTotalDaCarga = ct.PesoTotalDaCargaEmToneladas,
                Status = ct.Status
            };

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

            if (filtro.DataDeEmissaoInicial.HasValue)
            {
                queryable = queryable.Where(ct => ct.DataDeEmissao >= filtro.DataDeEmissaoInicial);
            }

            if (filtro.DataDeEmissaoFinal.HasValue)
            {
                DateTime dataDeEmissaoFinal = filtro.DataDeEmissaoFinal.Value;
                //como o registro no banco de dados está com data e hora preenchido, vou adicionar um dia na final e depois subtrair um tick.

                queryable = queryable.Where(ct => ct.DataDeEmissao <= dataDeEmissaoFinal.AddDays(1).AddMilliseconds(-1));
            }

            if (!string.IsNullOrEmpty(filtro.NumeroDoContrato))
            {
                queryable = queryable.Where(ct => ct.NumeroDoContrato.Contains(filtro.NumeroDoContrato));
            }

            if (filtro.Status.HasValue)
            {
                var statusDoConhecimentoDeTransporte = (Enumeradores.StatusDoConhecimentoDeTransporte) Enum.Parse(typeof(Enumeradores.StatusDoConhecimentoDeTransporte),Convert.ToString(filtro.Status.Value));
                queryable = queryable.Where(ct => ct.Status == statusDoConhecimentoDeTransporte);
            }

            var projecao = GerarProjecao();

            var kendoGridVm = new KendoGridVm
            {
                QuantidadeDeRegistros = queryable.Count(),
                Registros = queryable
                    .OrderByDescending(x => x.DataDeEmissao)
                    .Skip(paginacao.Skip)
                    .Take(paginacao.Take)
                    .Select(projecao).Cast<ListagemVm>().ToList(),
            };


            return kendoGridVm;
            

        }

        public ConhecimentoDeTransporteFormulario ObterRegistro(string chaveEletronica)
        {

            IQueryable<ConhecimentoDeTransporte> queryable = _session.Query<ConhecimentoDeTransporte>();
            
            return queryable
                .Where(x => x.ChaveEletronica == chaveEletronica)
                .Select(ct => new ConhecimentoDeTransporteFormulario
                {
                    ChaveEletronica = ct.ChaveEletronica,
                    DataDeEmissao = ct.DataDeEmissao.ToShortDateString(),
                    CodigoDoFornecedor = ct.Fornecedor.Codigo,
                    CodigoDaTransportadora = ct.Transportadora.Codigo,
                    NumeroDoConhecimento = ct.Numero,
                    Serie = ct.Serie,
                    NumeroDoContrato = ct.NumeroDoContrato,
                    ValorRealDoFrete = ct.ValorRealDoFrete,
                    PesoTotalDaCarga = ct.PesoTotalDaCargaEmToneladas,
                    Status = ct.Status,
                    PermiteAtribuir = ct.Status == Enumeradores.StatusDoConhecimentoDeTransporte.NaoAtribuido,
                    Erro = ct.MensagemDeErroDeProcessamento
                })
                .Single();

        }

        public KendoGridVm ListarNotasFiscais(string chaveEletronica)
        {
            IQueryable<ConhecimentoDeTransporte> queryable = _session.Query<ConhecimentoDeTransporte>();

            IQueryable<NotaFiscalDoConhecimentoDeTransporteVm> queryableDaProjecao = (from conhecimentoDeTransporte in queryable
                from notaFiscal in conhecimentoDeTransporte.NotasFiscais
                where conhecimentoDeTransporte.ChaveEletronica == chaveEletronica
                select new NotaFiscalDoConhecimentoDeTransporteVm
                {
                    Chave = notaFiscal.ChaveEletronica,
                    Numero = notaFiscal.Numero,
                    Serie = notaFiscal.Serie
                });


            var kendoGridVm = new KendoGridVm
            {
                QuantidadeDeRegistros = queryableDaProjecao.Count(),
                Registros = queryableDaProjecao.Cast<ListagemVm>().ToList()
            };

            return kendoGridVm;

        }

        public KendoGridVm ListarOrdensDeTransporte(string chaveEletronica)
        {
            IQueryable<ConhecimentoDeTransporte> queryable = _session.Query<ConhecimentoDeTransporte>();

            IQueryable<OrdemDeTransporteListagemVm> queryableDaProjecao = 
                (from conhecimentoDeTransporte in queryable
                 from ordemDeTransporte in conhecimentoDeTransporte.OrdensDeTransporte
                 where conhecimentoDeTransporte.ChaveEletronica == chaveEletronica
                 select new OrdemDeTransporteListagemVm
                 {
                     Id = ordemDeTransporte.Id,
                     CodigoDoFornecedor = ordemDeTransporte.Fornecedor.Codigo,
                     NomeDoFornecedor = ordemDeTransporte.Fornecedor.Nome,
                     Material = ordemDeTransporte.ProcessoDeCotacaoDeFrete.Produto.Descricao,
                     QuantidadeColetada = ordemDeTransporte.QuantidadeColetada,
                     QuantidadeLiberada = ordemDeTransporte.QuantidadeLiberada,
                     QuantidadeRealizada = ordemDeTransporte.QuantidadeRealizada
                 });

            var kendoGridVm = new KendoGridVm
            {
                QuantidadeDeRegistros = queryableDaProjecao.Count(),
                Registros = queryableDaProjecao.Cast<ListagemVm>().ToList()
            };

            return kendoGridVm;

        }
    }
}