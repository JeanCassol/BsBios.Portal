using System;
using System.Collections.Generic;
using BsBios.Portal.ViewModel;

namespace BsBios.Portal.Application.Queries.Contracts
{
    public interface IConsultaQuota
    {
        bool PossuiQuotaNaData(DateTime data);
        IList<QuotaConsultarVm> QuotasDaData(DateTime data);
        KendoGridVm ListarQuotasDoFornecedor(PaginacaoVm paginacaoVm, string codigoDoFornecedor);
        QuotaPorFornecedorVm ConsultarQuota(int idQuota);
        KendoGridVm ListarAgendamentosDaQuota(int idQuota);
        AgendamentoDeCargaCadastroVm ConsultarAgendamento(int idQuota, int idAgendamento);
        IList<NotaFiscalVm> NotasFiscaisDoAgendamento(int idQuota, int idAgendamento);
        KendoGridVm Consultar(PaginacaoVm paginacaoVm, ConferenciaDeCargaFiltroVm filtro);
    }
}