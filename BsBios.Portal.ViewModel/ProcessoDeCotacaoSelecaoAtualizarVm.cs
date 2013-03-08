using System.Collections.Generic;

namespace BsBios.Portal.ViewModel
{
    public class ProcessoDeCotacaoSelecaoAtualizarVm
    {
        public int IdProcessoCotacao { get; set; }
        public IList<CotacaoSelecaoVm> Cotacoes { get; set; }
    }
}
