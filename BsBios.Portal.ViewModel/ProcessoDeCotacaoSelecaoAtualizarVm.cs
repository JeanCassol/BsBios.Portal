using System.Collections.Generic;

namespace BsBios.Portal.ViewModel
{
    public class ProcessoDeCotacaoDeMaterialSelecaoAtualizarVm
    {
        public int IdProcessoCotacao { get; set; }
        public IList<CotacaoMaterialSelecaoVm> Cotacoes { get; set; }
    }
    public class ProcessoDeCotacaoDeFreteSelecaoAtualizarVm
    {
        public int IdProcessoCotacao { get; set; }
        public IList<CotacaoFreteSelecaoVm> Cotacoes { get; set; }
    }

}
