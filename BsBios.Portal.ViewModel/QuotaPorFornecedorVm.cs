using System;

namespace BsBios.Portal.ViewModel
{
    public class QuotaPorFornecedorVm: ListagemVm
    {
        public string CodigoTerminal { get; set; }
        public DateTime Data { get; set; }
        public string CodigoFornecedor { get; set; }
        public int CodigoMaterial { get; set; }
        public string DescricaoMaterial { get; set; }
        public string Fluxo { get; set; }
        public decimal PesoTotal { get; set; }
        public decimal PesoAgendado { get; set; }
        public decimal PesoDisponivel { get; set; }
    }
}
