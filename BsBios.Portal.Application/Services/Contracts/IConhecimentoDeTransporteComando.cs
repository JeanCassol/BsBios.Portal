namespace BsBios.Portal.Application.Services.Contracts
{
    public interface IConhecimentoDeTransporteComando
    {
        void AtribuirOrdemDeTransporte(string chaveDoConhecimento, int idDaOrdemDeTransporte);
    }
}