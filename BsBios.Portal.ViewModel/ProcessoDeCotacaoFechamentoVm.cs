using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace BsBios.Portal.ViewModel
{
    [DataContract]
    public class ProcessoDeCotacaoFechamentoVm
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
        public decimal ValorTotalSemImpostos { get; set; }
        [DataMember]
        public decimal ValorTotalComImpostos { get; set; }
        [DataMember]
        public bool Selecionada { get; set; }
        [DataMember]
        public decimal QuantidadeComprada { get; set; }
        
    }

    [CollectionDataContract]
    public class ListaProcessoDeCotacaoFechamento: List<ProcessoDeCotacaoFechamentoVm>{}

}
