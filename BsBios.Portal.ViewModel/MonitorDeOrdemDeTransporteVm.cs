using System.Collections.Generic;

namespace BsBios.Portal.ViewModel
{

    public class MonitorDeOrdemDeTransporteVm
    {
        public virtual string NumeroDoContrato { get; set; }
        public virtual string FornecedorDaMercadoria { get; set; }
        public virtual string Transportadora{ get; set; }
        public virtual string NumeroDaOrdemDeTransporte { get; set; }
        public virtual string MunicipioDeOrigem { get; set; }
        public virtual string MunicipioDeDestino { get; set; }
        public virtual decimal QuantidadeLiberada { get; set; }
        public virtual decimal QuantidadeRealizada { get; set; }
        public virtual decimal PercentualPendente { get; set; }
        public virtual decimal PercentualProjetado { get; set; }
        public virtual decimal QuantidadeEmTransito { get; set; }
        public virtual decimal PrevisaoDeChegadaNoDia { get; set; }
        public virtual decimal QuantidadePendente { get; set; }

        public virtual string ClasseDoSemaforo
        {
            get
            {
                string classe;
                if (PercentualProjetado < -15)
                {
                    classe = "semaforo-vermelho";
                }
                else if (PercentualProjetado < 0)
                {
                    classe = "semaforo-amarelo";
                }
                else
                {
                    classe = "semaforo-verde";

                }
                return classe;
            }
        }

    }

    public class MonitorDeOrdemDeTransporteListagemVm: MonitorDeOrdemDeTransporteVm
    {
        public virtual string Id { get; set; }
        public virtual string Material { get; set; }
    }

    public class MonitorDeOrdemDeTransportePorMaterialVm
    {
        public string Material { get; set; }
        public IList<MonitorDeOrdemDeTransporteVm> Registros { get; set; }

        public MonitorDeOrdemDeTransporteVm Total { get; set; }
    }

}