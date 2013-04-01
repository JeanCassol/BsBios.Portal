using System;
using System.Collections.Generic;
using System.Linq;
using BsBios.Portal.Application.Queries.Builders;
using BsBios.Portal.Application.Queries.Contracts;
using BsBios.Portal.Common;
using BsBios.Portal.Domain.Entities;
using BsBios.Portal.Infra.Repositories.Contracts;
using BsBios.Portal.ViewModel;

namespace BsBios.Portal.Application.Queries.Implementations
{
    public class ConsultaQuota : IConsultaQuota
    {
        private readonly IQuotas _quotas;
        private readonly IBuilder<Quota, QuotaConsultarVm> _builderQuota;
        private readonly IBuilder<Quota, QuotaPorFornecedorVm> _builderQuotaPorFornecedor;
        private readonly IBuilder<AgendamentoDeCarga, AgendamentoDeCargaListarVm> _builderAgendamentoDeCargaListar;
        private readonly IBuilder<AgendamentoDeCarga, AgendamentoDeCargaCadastroVm> _builderAgendamentoDeCargaCadastro;
        private readonly IBuilder<NotaFiscal, NotaFiscalVm> _builderNotaFiscal;

        public ConsultaQuota(IQuotas quotas, IBuilder<Quota, QuotaConsultarVm> builderQuota, 
            IBuilder<Quota, QuotaPorFornecedorVm> builderQuotaPorFornecedor, IBuilder<AgendamentoDeCarga, AgendamentoDeCargaListarVm> builderAgendamentoDeCargaListar, 
            IBuilder<AgendamentoDeCarga, AgendamentoDeCargaCadastroVm> builderAgendamentoDeCargaCadastro, IBuilder<NotaFiscal, NotaFiscalVm> builderNotaFiscal)
        {
            _quotas = quotas;
            _builderQuota = builderQuota;
            _builderQuotaPorFornecedor = builderQuotaPorFornecedor;
            _builderAgendamentoDeCargaListar = builderAgendamentoDeCargaListar;
            _builderAgendamentoDeCargaCadastro = builderAgendamentoDeCargaCadastro;
            _builderNotaFiscal = builderNotaFiscal;
        }

        public bool PossuiQuotaNaData(DateTime data)
        {
            return _quotas.FiltraPorData(data).Count() > 0;
        }

        public IList<QuotaConsultarVm> QuotasDaData(DateTime data)
        {
            return _builderQuota.BuildList(_quotas.FiltraPorData(data).List());
        }

        public KendoGridVm ListarQuotasDoFornecedor(PaginacaoVm paginacaoVm, string codigoDoFornecedor)
        {
            _quotas.DoFornecedor(codigoDoFornecedor);
            return new KendoGridVm
                {
                    QuantidadeDeRegistros = _quotas.Count() ,
                    Registros = _builderQuotaPorFornecedor.BuildList(_quotas
                    .GetQuery()
                    .OrderByDescending(x => x.Data)
                    .Skip(paginacaoVm.Skip)
                    .Take(paginacaoVm.Take).ToList())
                            .Cast<ListagemVm>()
                            .ToList()
                             
                };
        }

        public QuotaPorFornecedorVm ConsultarQuota(int idQuota)
        {
            return _builderQuotaPorFornecedor.BuildSingle(_quotas.BuscaPorId(idQuota));
        }

        public KendoGridVm ListarAgendamentosDaQuota(int idQuota)
        {
            Quota quota = _quotas.BuscaPorId(idQuota);
            return new KendoGridVm
                {
                    QuantidadeDeRegistros = quota.Agendamentos.Count,
                    Registros = _builderAgendamentoDeCargaListar.BuildList(quota.Agendamentos ).Cast<ListagemVm>().ToList()
                };
        }

        public AgendamentoDeCargaCadastroVm ConsultarAgendamento(int idQuota, int idAgendamento)
        {
            Quota quota = _quotas.BuscaPorId(idQuota);
            AgendamentoDeCarga agendamentoDeCarga = quota.Agendamentos.Single(x => x.Id == idAgendamento);
            return _builderAgendamentoDeCargaCadastro.BuildSingle(agendamentoDeCarga);
        }

        public IList<NotaFiscalVm> NotasFiscaisDoAgendamento(int idQuota, int idAgendamento)
        {
            Quota quota = _quotas.BuscaPorId(idQuota);
            var agendamentoDeCarga = (AgendamentoDeDescarregamento) quota.Agendamentos.Single(x => x.Id == idAgendamento);
            return _builderNotaFiscal.BuildList(agendamentoDeCarga.NotasFiscais);
        }

        public KendoGridVm Consultar(PaginacaoVm paginacaoVm, ConferenciaDeCargaFiltroVm filtro)
        {
            bool? realizado = null;
            if (filtro.CodigoRealizacaoDeAgendamento.HasValue)
            {
                realizado = ((Enumeradores.RealizacaoDeAgendamento) Enum.Parse(typeof (Enumeradores.RealizacaoDeAgendamento),
                    Convert.ToString(filtro.CodigoRealizacaoDeAgendamento.Value)) == Enumeradores.RealizacaoDeAgendamento.Realizado) ;
            }
            if (!string.IsNullOrEmpty(filtro.Placa))
            {
                filtro.Placa = filtro.Placa.ToLower().Replace("-","");    
            }
            
            var queryDescarregamento = (from quota in _quotas.GetQuery()
                     from agendamento in quota.Agendamentos
                     from notaFiscal in ((AgendamentoDeDescarregamento) agendamento).NotasFiscais
                     where (!realizado.HasValue || agendamento.Realizado == realizado)
                     && (string.IsNullOrEmpty(filtro.Placa) || agendamento.Placa.ToLower().Replace("-", "") == filtro.Placa)
                           && (string.IsNullOrEmpty(filtro.DataAgendamento) || quota.Data == Convert.ToDateTime(filtro.DataAgendamento))
                           && quota.CodigoTerminal == filtro.CodigoTerminal
                           && ( string.IsNullOrEmpty(filtro.NumeroNf) || notaFiscal.Numero == filtro.NumeroNf)
                     select new 
                         {
                             IdQuota = quota.Id,
                             IdAgendamento = agendamento.Id,
                             DataAgendamento = quota.Data,
                             quota.Material ,
                             quota.FluxoDeCarga ,
                             agendamento.Placa,
                             notaFiscal.CnpjDoEmitente,
                             NumeroNf = notaFiscal.Numero
                         });

            var query = queryDescarregamento.AsEnumerable();

            if (string.IsNullOrEmpty(filtro.NumeroNf))
            {
                var queryCarregamento = (from quota in _quotas.GetQuery()
                                         from agendamento in quota.Agendamentos
                                         where agendamento is AgendamentoDeCarregamento
                                         && (!realizado.HasValue || agendamento.Realizado == realizado)
                                         && (string.IsNullOrEmpty(filtro.Placa) || agendamento.Placa.ToLower().Replace("-", "") == filtro.Placa)
                                               && (string.IsNullOrEmpty(filtro.DataAgendamento) || quota.Data == Convert.ToDateTime(filtro.DataAgendamento))
                                               && quota.CodigoTerminal == filtro.CodigoTerminal
                                         select new
                                         {
                                             IdQuota = quota.Id,
                                             IdAgendamento = agendamento.Id,
                                             DataAgendamento = quota.Data,
                                             quota.Material,
                                             quota.FluxoDeCarga,
                                             agendamento.Placa,
                                             CnpjDoEmitente = "",
                                             NumeroNf = ""
                                         });

                query = query.Union(queryCarregamento.AsEnumerable());

            }

            return new KendoGridVm()
                {
                    QuantidadeDeRegistros = query.Count(),
                    Registros = query.ToList()
                    .Skip(paginacaoVm.Skip)
                    .Take(paginacaoVm.Take)
                    .Select(x => 
                    new ConferenciaDeCargaPesquisaResultadoVm
                        {
                            IdQuota = x.IdQuota,
                            IdAgendamento = x.IdAgendamento,
                            DescricaoMaterial = x.Material.Descricao() ,
                            DescricaoFluxo = x.FluxoDeCarga.Descricao(),
                            DataAgendamento = x.DataAgendamento.ToShortDateString(),
                            Placa = x.Placa ,
                            CnpjEmitente = x.CnpjDoEmitente ,
                            NumeroNf = x.NumeroNf
                        }).Cast<ListagemVm>().ToList()
                };
        }
    }
}