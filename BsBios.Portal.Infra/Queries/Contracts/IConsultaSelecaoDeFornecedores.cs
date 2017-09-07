using System.Collections.Generic;

namespace BsBios.Portal.Infra.Queries.Contracts
{
    public interface IConsultaSelecaoDeFornecedores
    {
        IList<SelecaoDeFornecedoresVm> Listar(); 
    }
}