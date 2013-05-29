using System.Collections.Generic;
using System.Runtime.Serialization;

namespace BsBios.Portal.ViewModel
{
    [DataContract]
    public class ProcessoDeCotacaoDeMaterialFechamentoComunicacaoSapVm
    {
        [DataMember]
        public string NumeroRequisicao { get; set; }
        [DataMember]
        public string NumeroItem { get; set; }
        [DataMember]
        public string CodigoFornecedor { get; set; }
        [DataMember]
        public string CodigoCondicaoPagamento { get; set; }
        [DataMember]
        public string CodigoIva { get; set; }
        [DataMember]
        public string CodigoIncoterm1 { get; set; }
        [DataMember]
        public string Incoterm2 { get; set; }
        [DataMember]
        public decimal Preco { get; set; }
        [DataMember]
        public bool Selecionada { get; set; }
        [DataMember]
        public decimal QuantidadeComprada { get; set; }
        
    }

    [CollectionDataContract]
    public class ListaProcessoDeCotacaoDeMaterialFechamento: List<ProcessoDeCotacaoDeMaterialFechamentoComunicacaoSapVm>{}

}
