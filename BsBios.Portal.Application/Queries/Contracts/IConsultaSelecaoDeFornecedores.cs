using System.Collections.Generic;

namespace BsBios.Portal.Application.Queries.Contracts
{
    public interface IConsultaSelecaoDeFornecedores
    {
        IList<SelecaoDeFornecedoresVm> Listar(); 
    }
}