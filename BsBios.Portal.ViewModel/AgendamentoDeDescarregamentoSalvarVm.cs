using System.Collections.Generic;

namespace BsBios.Portal.ViewModel
{
    public class AgendamentoDeDescarregamentoSalvarVm : AgendamentoDeCargaCadastroVm
    {
        public IList<NotaFiscalVm> NotasFiscais { get; set; }
    }
}
