namespace BsBios.Portal.ViewModel
{
    /// <summary>
    /// classe utilizada quando a lista de fornecedores do processo de cotação é atualizada
    /// </summary>
    public class ProcessoDeCotacaoFornecedoresAtualizarVm
    {
        public int IdProcessoCotacao { get; set; }
        public string[] CodigoFornecedoresSelecionados { get; set; }
    }
}
