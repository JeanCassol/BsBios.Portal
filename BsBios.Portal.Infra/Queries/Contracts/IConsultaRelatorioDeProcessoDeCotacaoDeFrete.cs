using System.Collections.Generic;
using BsBios.Portal.ViewModel;

namespace BsBios.Portal.Infra.Queries.Contracts
{
    public interface IConsultaRelatorioDeProcessoDeCotacaoDeFrete
    {
        IList<RelatorioDeProcessoDeCotacaoDeFreteAnaliticoVm> ListagemAnalitica(RelatorioDeProcessoDeCotacaoDeFreteFiltroVm filtro);
        IList<RelatorioDeProcessoDeCotacaoDeFreteSinteticoVm> ListagemSinteticaComSoma(RelatorioDeProcessoDeCotacaoDeFreteFiltroVm filtro);
        IList<RelatorioDeProcessoDeCotacaoDeFreteSinteticoVm> ListagemSinteticaComMedia(RelatorioDeProcessoDeCotacaoDeFreteFiltroVm filtro);
    }
}