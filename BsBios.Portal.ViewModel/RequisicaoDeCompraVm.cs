using System;
using System.Runtime.Serialization;

namespace BsBios.Portal.ViewModel
{
    [DataContract]
    public class RequisicaoDeCompraVm
    {
        [DataMember]
        public string NumeroRequisicao { get; set; }
        [DataMember]
        public int NumeroItem { get; set; }
        [DataMember]
        public string Descricao { get; set; }
        [DataMember]
        public string Material { get; set; }
        [DataMember]
        public decimal Quantidade { get; set; }
        [DataMember]
        public string UnidadeMedida { get; set; }
        [DataMember]
        public string Centro { get; set; }
        [DataMember]
        public DateTime DataDeSolicitacao { get; set; }
        [DataMember]
        public DateTime DataDeLiberacao { get; set; }
        [DataMember]
        public DateTime DataDeRemessa { get; set; }
        [DataMember]
        public string FornecedorPretendido { get; set; }
        [DataMember]
        public string Requisitante { get; set; }
        [DataMember]
        public string Criador { get; set; }
    }
}
