using System.Collections.Generic;
using System.Runtime.Serialization;

namespace BsBios.Portal.ViewModel
{
    [DataContract]
    public class NotaFiscalDoConhecimentoDeTransporteVm: ListagemVm
    {
        [DataMember]
        public string Chave { get; set; }
        [DataMember]
        public string Numero { get; set; }
        [DataMember]
        public string Serie { get; set; }
    }

    [CollectionDataContract]
    public class ListaDeNotaFiscalDeConhecimentoDeTransporte : List<NotaFiscalDoConhecimentoDeTransporteVm> { }

    [DataContract]
    public class ConhecimentoDeTransporteVm
    {
        [DataMember]
        public string ChaveEletronica { get; set; }
        [DataMember]
        public string CnpjDoFornecedor { get; set; }
        [DataMember]
        public string CnpjDaTransportadora { get; set; }
        [DataMember]
        public string DataDeEmissao { get; set; }
        [DataMember]
        public string Serie { get; set; }
        [DataMember]
        public string Numero { get; set; }
        [DataMember]
        public string NumeroDoContrato { get; set; }
        [DataMember]
        public decimal ValorRealDoFrete { get; set; }
        [DataMember]
        public decimal PesoTotalDaCarga { get; set; }

        [DataMember]
        public ListaDeNotaFiscalDeConhecimentoDeTransporte NotasFiscais { get; set; }
    }

    [CollectionDataContract]
    public class ListaDeConhecimentoDeTransporte : List<ConhecimentoDeTransporteVm> { }

}