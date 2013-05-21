namespace BsBios.Portal.Infra.Model
{
    public class ContaDeEmail
    {
        public string EmailDoRemetente{ get; protected set; }
        /// <summary>
        /// domínio do usuário
        /// </summary>
        public string Dominio { get; protected set; }
        public string Usuario { get; protected set; }
        public string Senha { get; protected set; }
        public string ServidorSmtp { get; protected set; }
        public int Porta { get; protected set; }
        public bool HabilitarSsl { get; protected set; }

        public ContaDeEmail(string emailDoRemetente, string dominio, string usuario, string senha, 
            string servidorSmtp, int porta, bool habilitarSsl)
        {
            EmailDoRemetente = emailDoRemetente;
            Dominio = dominio;
            Usuario = usuario;
            Senha = senha;
            ServidorSmtp = servidorSmtp;
            Porta = porta;
            HabilitarSsl = habilitarSsl;
        }
    }
}
