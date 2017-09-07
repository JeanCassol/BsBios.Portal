using System;

namespace BsBios.Portal.Common.DTO
{
    public class FiltroDeConhecimentoDeTransporte
    {
        public string CodigoDoFornecedor { get; set; }
        public string CodigoDaTransportadora { get; set; }
        public DateTime? DataDeEmissaoInicial { get; set; }
        public DateTime? DataDeEmissaoFinal { get; set; }
        public string NumeroDoContrato { get; set; }
        public int? Status { get; set; }
    }
}
