using System.Configuration;

namespace BsBios.Portal.Infra.Model
{
    public class CredencialSap: ConfigurationSection
    {
        [ConfigurationProperty("EnderecoDoServidor", IsRequired = true)]
        public string EnderecoDoServidor
        {
            get
            {
                return this["EnderecoDoServidor"] as string;
            }
        }
        [ConfigurationProperty("Usuario", IsRequired = true)]
        public string Usuario
        {
            get
            {
                return this["Usuario"] as string;
            }
        }
        [ConfigurationProperty("Senha", IsRequired = true)]
        public string Senha
        {
            get
            {
                return this["Senha"] as string;
            }
        }


    }
}