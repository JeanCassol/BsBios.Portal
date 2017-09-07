namespace BsBios.Portal.Domain.Repositories
{
    public interface IProcessosDeCotacaoDeMaterial: IProcessosDeCotacao
    {
        //IProcessosDeCotacao GeradosPelaRequisicaoDeCompra(string numeroDaRequisicao, string numeroDoItem);
        IProcessosDeCotacao GeradosPelaRequisicaoDeCompra(int idRequisicaoDeCompra);
    }
}