using System.Collections.Generic;

namespace BsBios.Portal.ViewModel
{
    public class ColetaSalvarVm: ColetaListagemVm
    {
        public ColetaSalvarVm()
        {
            NotasFiscais = new List<NotaFiscalDeColetaVm>();
        }
        public IList<NotaFiscalDeColetaVm> NotasFiscais { get; set; }
    }
}