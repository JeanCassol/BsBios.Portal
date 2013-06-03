using BsBios.Portal.Domain.Entities;
using BsBios.Portal.ViewModel;

namespace BsBios.Portal.Infra.Queries.Builders
{
    public class QuotaPesoBuilder : Builder<Quota, QuotaPesoVm>
    {
        public override QuotaPesoVm BuildSingle(Quota model)
        {
            return new QuotaPesoVm
                {
                    PesoTotal  = model.PesoTotal,
                    PesoAgendado = model.PesoAgendado ,
                    PesoDisponivel = model.PesoDisponivel 
                };
        }
    }
}
