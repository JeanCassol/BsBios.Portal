using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace BsBios.Portal.ViewModel
{
    [DataContract]
    public class FornecedorCadastroVm:ListagemVm
    {
        [DataMember]
        [Display(Name = "Código: ")]
        public string Codigo { get; set; }
        [DataMember]
        [Display(Name = "Nome: ")]
        public string Nome { get; set; }
        [DataMember]
        [Display(Name = "E-mail: ")]
        public string Email { get; set; }
        [DataMember]
        [Display(Name = "CNPJ: ")]
        public string Cnpj { get; set; }
        [DataMember]
        [Display(Name = "Municipio: ")]
        public string Municipio { get; set; }
        [DataMember]
        [Display(Name = "UF: ")]
        public string Uf { get; set; }
    }

    [CollectionDataContract]
    public class ListaFornecedores:List<FornecedorCadastroVm>{}
}
