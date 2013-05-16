using System.ComponentModel;

namespace BsBios.Portal.ViewModel
{
    public class CotacaoMaterialItemCadastroVm : CotacaoMaterialItemInformarVm
    {

        [DisplayName("Material: ")]
        public string Material { get; set; }
        [DisplayName("Quantidade: ")]
        public decimal Quantidade { get; set; }
        [DisplayName("Unidade de Medida: ")]
        public string UnidadeDeMedida { get; set; }
        
    }
}
