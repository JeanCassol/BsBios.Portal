using System.ComponentModel;

namespace BsBios.Portal.Common.DTO
{
    public class ConhecimentoDeTransporteFormulario: ConhecimentoDeTransporteListagem
    {
        [DisplayName("Série")]
        public string Serie { get; set; }

        public bool PermiteAtribuir { get; set; }
        public bool PermiteReprocessar { get; set; }
        [DisplayName("Descrição do Erro")]
        public string Erro { get; set; }
    }
}
