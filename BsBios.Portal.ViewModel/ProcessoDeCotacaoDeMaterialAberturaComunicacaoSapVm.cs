using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace BsBios.Portal.ViewModel
{
    /// <summary>
    /// Classe utilizada para enviar dados para o SAP quando os fornecedore são selecionados e o processo de cotação é iniciado.
    /// </summary>
    [DataContract]
    public class ProcessoDeCotacaoDeMaterialAberturaComunicacaoSapVm
    {
        [DataMember]
        public int IdProcessoCotacao { get; set; } 
        //[DataMember]
        //public DateTime DataCriacao { get; set; }
        [DataMember]
        public string CodigoFornecedor { get; set; }
        [DataMember]
        public DateTime DataLimiteRetorno { get; set; }

        [DataMember]
        public ListaProcessoDeCotacaoDeMaterialAberturaItemVm Itens { get; set; }
    }

    public  class ProcessoDeCotacaoDeMaterialAberturaItemVm
    {
        [DataMember]
        public string NumeroRequisicao { get; set; }
        [DataMember]
        public string NumeroItem { get; set; }
    }

    [CollectionDataContract]
    public class ListaProcessoDeCotacaoDeMaterialAberturaItemVm : List<ProcessoDeCotacaoDeMaterialAberturaItemVm>{}

    //[CollectionDataContract]
    //public class ListaProcessoDeCotacaoDeMaterialAbertura: List<ProcessoDeCotacaoDeMaterialAberturaComunicacaoSapVm>{}
}
