using System.Collections.Generic;

namespace BsBios.Portal.ViewModel
{
    public class ColetaSalvarVm: ColetaListagemVm
    {
        public IList<NotaFiscalDeColetaVm> NotasFiscais { get; set; }
    }
}