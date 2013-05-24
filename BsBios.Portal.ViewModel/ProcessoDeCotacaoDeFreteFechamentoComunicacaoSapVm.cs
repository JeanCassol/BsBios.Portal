using System.Collections.Generic;
using System.Runtime.Serialization;

namespace BsBios.Portal.ViewModel
{
    [DataContract]
    public class ProcessoDeCotacaoDeFreteFechamentoComunicacaoSapVm
    {
        [DataMember]
        public string CodigoTransportadora { get; set; }
        [DataMember]
        public string CodigoItinerario { get; set; }
        [DataMember]
        public string CodigoMaterial { get; set; }
        [DataMember]
        public string CodigoUnidadeMedida { get; set; }
        [DataMember]
        public string NumeroDoContrato { get; set; }
        [DataMember]
        public decimal Valor { get; set; }
        [DataMember]
        public string DataDeValidadeInicial { get; set; }
        [DataMember]
        public string DataDeValidaFinal { get; set; }
       
    }

    //[KnownType(typeof(ListaProcessoDeCotacaoDeFreteFechamento))]
    [CollectionDataContract]
    public class ListaProcessoDeCotacaoDeFreteFechamento : List<ProcessoDeCotacaoDeFreteFechamentoComunicacaoSapVm> { }

}
