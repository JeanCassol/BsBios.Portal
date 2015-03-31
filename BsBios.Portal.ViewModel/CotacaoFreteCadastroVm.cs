using System.ComponentModel;

namespace BsBios.Portal.ViewModel
{
    public class CotacaoFreteCadastroVm : CotacaoInformarVm
    {
        public ProcessoDeCotacaoDeFreteCabecalhoVm Cabecalho { get; set; }

        /// <summary>
        /// usuada para indicar se os campos da tela serão habilitados para edição
        /// </summary>
        public bool PermiteEditar { get; set; }
        public bool PermiteAlterarPreco { get; set; }
        public int IdFornecedorParticipante { get; set; }

    }
}