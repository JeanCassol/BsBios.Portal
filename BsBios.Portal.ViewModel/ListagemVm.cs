using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BsBios.Portal.ViewModel
{
    public class ListagemVm<TViewModel>
    {
        public IList<TViewModel> Registros { get; set; }
        public int TotalDeRegistros;
    }
}
