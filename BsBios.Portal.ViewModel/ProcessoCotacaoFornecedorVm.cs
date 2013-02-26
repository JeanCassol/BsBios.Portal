namespace BsBios.Portal.ViewModel
{
    public class ProcessoCotacaoFornecedorVm:ListagemVm
    {
        public string Codigo { get; set; }
        public string Nome { get; set; }
        public decimal Preco { get; set; }
        public string Selecionado { get; set; }
    }
}
