using BsBios.Portal.Common;
using BsBios.Portal.ViewModel;

namespace BsBios.Portal.Application.DTO
{
    public class ConhecimentoDeTransporteListagem: ListagemVm
    {
        public string ChaveEletronica { get; set; }
        public string DataDeEmissao { get; set; }
        public string CodigoDoFornecedor { get; set; }
        public string CodigoDaTransportadora { get; set; }
        public string NumeroDoConhecimento { get; set; }
        public string NumeroDoContrato { get; set; }
        public decimal ValorRealDoFrete { get; set; }
        public decimal PesoTotalDaCarga { get; set; }
        public Enumeradores.StatusDoConhecimentoDeTransporte Status { get; set; }

        public string DescricaoDoStatus
        {
            get { return Status.Descricao(); }
        }
    }
}
