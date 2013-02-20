using System;
using System.Runtime.Serialization;

namespace BsBios.Portal.ViewModel
{
    [DataContract]
    public class RequisicaoDeCompraVm
    {
        public string Numero { get; set; }
        public int NumeroItem { get; set; }
        public string Descricao { get; set; }
        public string Material { get; set; }
        public decimal Quantidade { get; set; }
        public string UnidadeMedida { get; set; }
        public string Centro { get; set; }
        public DateTime DataDeSolicitacao { get; set; }
        public DateTime DataDeLiberacao { get; set; }
        public DateTime DataDeRemessa { get; set; }
        public string FornecedorPretendido { get; set; }
        public string Requisitante { get; set; }
        public string Criador { get; set; }
    }
}
