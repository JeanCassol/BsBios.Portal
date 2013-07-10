using System.Collections.Generic;
using System.Runtime.Serialization;

namespace BsBios.Portal.ViewModel
{
    [DataContract]
    public class ProcessoDeCotacaoDeMaterialFechamentoComunicacaoSapVm
    {
        [DataMember]
        public int IdProcessoCotacao { get; set; }
        [DataMember]
        public int DocumentoDoSap { get; set; }  // 1 = pedido; 2 =  contrato
        [DataMember]
        public string TextoDeCabecalho { get; set; }
        [DataMember]
        public string NotaDeCabecalho { get; set; }
        [DataMember]
        public string NumeroCotacao { get; set; }
        [DataMember]
        public string CodigoFornecedor { get; set; }
        [DataMember]
        public string CodigoCondicaoPagamento { get; set; }
        [DataMember]
        public string CodigoIncoterm1 { get; set; }
        [DataMember]
        public string Incoterm2 { get; set; }
        [DataMember]
        public ListaProcessoDeCotacaoDeMaterialFechamentoItemVm Itens { get; set; }
        
    }

    public  class ProcessoDeCotacaoDeMaterialFechamentoItemVm
    {
        [DataMember]
        public string NumeroRequisicao { get; set; }
        [DataMember]
        public string NumeroItem { get; set; }
        [DataMember]
        public decimal QuantidadeComprada { get; set; }
        [DataMember]
        public decimal Preco { get; set; }
        [DataMember]
        public string CodigoIva { get; set; }
        [DataMember]
        public bool Selecionada { get; set; }
    }

    [CollectionDataContract]
    public class ListaProcessoDeCotacaoDeMaterialFechamentoItemVm:List<ProcessoDeCotacaoDeMaterialFechamentoItemVm>{}

    //[CollectionDataContract]
    //public class ListaProcessoDeCotacaoDeMaterialFechamento: List<ProcessoDeCotacaoDeMaterialFechamentoComunicacaoSapVm>{}

}
