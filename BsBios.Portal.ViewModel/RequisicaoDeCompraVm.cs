using System.ComponentModel;
using System.Runtime.Serialization;

namespace BsBios.Portal.ViewModel
{
    [DataContract]
    public class RequisicaoDeCompraVm
    {
        [DataMember]
        [DisplayName("Número da Requisição: ")]
        public string NumeroRequisicao { get; set; }
        [DataMember]
        [DisplayName("Item da Requisição: ")]
        public string NumeroItem { get; set; }
        [DataMember]
        [DisplayName("Descrição: ")]
        public string Descricao { get; set; }
        [DataMember]
        [DisplayName("Material: ")]
        public string Material { get; set; }
        [DataMember]
        [DisplayName("Quantidade: ")]
        public decimal Quantidade { get; set; }
        [DataMember]
        [DisplayName("Unidade de Medida: ")]
        public string UnidadeMedida { get; set; }
        [DataMember]
        [DisplayName("Centro de Custo: ")]
        public string Centro { get; set; }
        [DataMember]
        [DisplayName("Data de Solicitação: ")]
        public string DataDeSolicitacao { get; set; }
        [DataMember]
        [DisplayName("Data de Liberação: ")]
        public string DataDeLiberacao { get; set; }
        [DataMember]
        [DisplayName("Data de Remessa: ")]
        public string DataDeRemessa { get; set; }
        [DataMember]
        [DisplayName("Fornecedor Pretendido: ")]
        public string FornecedorPretendido { get; set; }
        [DataMember]
        [DisplayName("Requisitante: ")]
        public string Requisitante { get; set; }
        [DataMember]
        [DisplayName("Criado por: ")]
        public string Criador { get; set; }
    }

}
