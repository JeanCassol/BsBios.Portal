using BsBios.Portal.Domain.Entities;

namespace BsBios.Portal.Domain.Services.Contracts
{
    public interface IProcessadorDeNotaFiscalMiro
    {
        OrdemDeTransporte Processar(NotaFiscalMiro notaFiscal);
    }
}