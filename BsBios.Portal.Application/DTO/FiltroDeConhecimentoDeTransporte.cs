using System;

namespace BsBios.Portal.Application.DTO
{
    public class FiltroDeConhecimentoDeTransporte
    {
        public string CodigoDoFornecedor { get; set; }
        public string CodigoDaTransportadora { get; set; }
        public DateTime? DataDeEmissao { get; set; }
        public string NumeroDoContrato { get; set; }
        public int? Status { get; set; }
    }
}
