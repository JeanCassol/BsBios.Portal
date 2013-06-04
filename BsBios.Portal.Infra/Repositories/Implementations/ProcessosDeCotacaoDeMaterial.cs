using BsBios.Portal.Common;
using BsBios.Portal.Infra.Repositories.Contracts;

namespace BsBios.Portal.Infra.Repositories.Implementations
{
    public class ProcessosDeCotacaoDeMaterial : ProcessosDeCotacao, IProcessosDeCotacaoDeMaterial
    {
        public ProcessosDeCotacaoDeMaterial(IUnitOfWorkNh unitOfWork) : base(unitOfWork)
        {
            FiltraPorTipo(Enumeradores.TipoDeCotacao.Material);
        }
    }
}