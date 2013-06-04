using System.Collections.Generic;

namespace BsBios.Portal.ViewModel
{
    public class RelatorioEficienciaNegociacaoVm
    {
        public string Login { get; set; }
        public string Nome { get; set; }
        public IEnumerable<ProcessoDeCotacaoValoresVm> Valores { get; set; }
    }
}