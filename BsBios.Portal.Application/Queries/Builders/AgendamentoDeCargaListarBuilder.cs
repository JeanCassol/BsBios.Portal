using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BsBios.Portal.Domain.Entities;
using BsBios.Portal.ViewModel;

namespace BsBios.Portal.Application.Queries.Builders
{
    public class AgendamentoDeCargaListarBuilder : Builder<AgendamentoDeCarga, AgendamentoDeCargaListarVm>
    {
        public override AgendamentoDeCargaListarVm BuildSingle(AgendamentoDeCarga model)
        {
            return new AgendamentoDeCargaListarVm
                {
                    IdQuota = model.Quota.Id ,
                    IdAgendamento = model.Id,
                    Peso = model.PesoTotal ,
                    Placa =  model.Placa,
                    Realizado = model.Realizado ? "Sim": "Não" 
                };
        }
    }
}
