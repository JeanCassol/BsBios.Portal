namespace BsBios.Portal.Application.Services.Contracts
{
    public interface ICadastroProdutoFornecedor
    {
        void AtualizarFornecedoresDoProduto(string codigoProduto, string[] codigoDosFornecedores);         
    }
}