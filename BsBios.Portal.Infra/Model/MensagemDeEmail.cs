namespace BsBios.Portal.Infra.Model
{
    public class MensagemDeEmail
    {
        public string Assunto { get; protected set; }
        public string Conteudo{ get; protected set; }

        public MensagemDeEmail(string assunto, string conteudo)
        {
            Assunto = assunto;
            Conteudo = conteudo;
        }
    }
}
