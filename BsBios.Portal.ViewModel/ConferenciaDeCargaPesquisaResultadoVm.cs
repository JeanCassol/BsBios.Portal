using System;

namespace BsBios.Portal.ViewModel
{
    public class ConferenciaDeCargaPesquisaResultadoVm:ListagemVm
    {
        public virtual string Id { get; set; }
        public virtual string CodigoTerminal { get; set; }
        public virtual string DescricaoTerminal { get; set; }
        public virtual int? IdQuota { get; set; }
        public virtual int? IdAgendamento { get; set; }

        public virtual int? IdOrdemTransporte { get; set; }
        public virtual int? IdColeta { get; set; }
        public virtual DateTime DataAgendamento { get; set; }

        public virtual string DataDeAgendamentoFormatada
        {
            get
            {
                return DataAgendamento.ToShortDateString();
            }
        }
        public virtual string Placa { get; set; }
        public virtual string NumeroNf { get; set; }
        public virtual string CnpjEmitente { get; set; }
        public virtual string NomeEmitente { get; set; }
        public virtual string CodigoDeposito { get; set; }
        public virtual string DescricaoMaterial { get; set; }
        public virtual string DescricaoFluxo { get; set; }
        public virtual bool Realizado { get; set; }

    }

}
