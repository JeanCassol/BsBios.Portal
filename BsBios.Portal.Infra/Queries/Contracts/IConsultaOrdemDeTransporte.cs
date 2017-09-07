﻿using System.Collections.Generic;
using BsBios.Portal.Infra.Model;
using BsBios.Portal.ViewModel;

namespace BsBios.Portal.Infra.Queries.Contracts
{
    public interface IConsultaOrdemDeTransporte
    {
        KendoGridVm Listar(PaginacaoVm paginacao, OrdemDeTransporteListagemFiltroVm filtro);
        OrdemDeTransporteCadastroVm ConsultarOrdem(int id);

        KendoGridVm ListarColetas(PaginacaoVm paginacao, int idDaOrdemDeTransporte);

        ColetaVm ConsultaColeta(int idDaOrdemDeTransporte, int idDaColeta, UsuarioConectado usuarioConectado);

        IList<NotaFiscalDeColetaVm> NotasFiscaisDaColeta(int iddDaOrdemDeTransporte, int idColeta);

        decimal CalcularQuantidadeLiberadaPeloProcessoDeCotacao(int idDoProcessoDeCotacaoDeFrete);
    }
}