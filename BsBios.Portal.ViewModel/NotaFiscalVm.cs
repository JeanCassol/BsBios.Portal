using System;

namespace BsBios.Portal.ViewModel
{
    public class NotaFiscalVm
    {
        public string Numero { get; set; }
        public string Serie { get; set; }
        public DateTime DataDeEmissao { get; set; }
        public string NomeDoEmitente { get; set; }
        public string CnpjDoEmitente { get; set; }
        public string NomeDoContratante { get; set; }
        public string CnpjDoContratante { get; set; }
        public string  NumeroDoContrato { get; set; }
        public decimal Valor { get; set; }
        public decimal Peso { get; set; }
    }
}
