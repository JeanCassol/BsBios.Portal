using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace BsBios.Portal.ViewModel
{
    [DataContract]
    public class CotacaoInicioVm
    {
        [DataMember]
        public DateTime DataCriacao { get; set; }
        [DataMember]
        public string CodigoFornecedor { get; set; }
        [DataMember]
        public DateTime DataLimiteRetorno { get; set; }
        [DataMember]
        public string NumeroRequisicao { get; set; }
        [DataMember]
        public string NumeroItem { get; set; }
    }
    [CollectionDataContract]
    public class ListaCotacaoInicio: List<CotacaoInicioVm>{}
}
