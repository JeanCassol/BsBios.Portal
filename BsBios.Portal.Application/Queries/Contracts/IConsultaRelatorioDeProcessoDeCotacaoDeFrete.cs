﻿using System.Collections;
using System.Collections.Generic;
using BsBios.Portal.ViewModel;

namespace BsBios.Portal.Application.Queries.Contracts
{
    public interface IConsultaRelatorioDeProcessoDeCotacaoDeFrete
    {
        IList<RelatorioDeProcessoDeCotacaoDeFreteAnaliticoVm> ListagemAnalitica(RelatorioDeProcessoDeCotacaoDeFreteFiltroVm filtro);
        IList<RelatorioDeProcessoDeCotacaoDeFreteSinteticoVm> ListagemSinteticaComSoma(RelatorioDeProcessoDeCotacaoDeFreteFiltroVm filtro);
        IList<RelatorioDeProcessoDeCotacaoDeFreteSinteticoVm> ListagemSinteticaComMedia(RelatorioDeProcessoDeCotacaoDeFreteFiltroVm filtro);
    }
}