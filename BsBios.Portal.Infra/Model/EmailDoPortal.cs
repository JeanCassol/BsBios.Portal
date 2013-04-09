using System.Configuration;

namespace BsBios.Portal.Infra.Model
{
    public class EmailDoPortal : ConfigurationSection
    {
        [ConfigurationProperty("RemetenteSuprimentos", DefaultValue = "compras@bsbios.com", IsRequired = false)]
        public string RemetenteSuprimentos
        {
            get
            {
                return this["RemetenteSuprimentos"] as string;
            }
        }

        [ConfigurationProperty("RemetenteLogistica", DefaultValue = "logistica.pf@bsbios.com", IsRequired = false)]
        public string RemetenteLogistica
        {
            get
            {
                return this["RemetenteLogistica"] as string;
            }
        }

        [ConfigurationProperty("Servidor", DefaultValue = "mail.bsbios.com", IsRequired = false)]
        public string Servidor
        {
            get
            {
                return this["Servidor"] as string;
            }
        }

        [ConfigurationProperty("Porta", DefaultValue = 25, IsRequired = false)]
        public int Porta
        {
            get
            {
                return (int) this["Porta"] ;
            }
        }

        [ConfigurationProperty("Dominio", DefaultValue = "bsbios.com", IsRequired = false)]
        public string Dominio
        {
            get
            {
                return this["Dominio"] as string;
            }
        }

        [ConfigurationProperty("Usuario", DefaultValue = "sistemas", IsRequired = false)]
        public string Usuario
        {
            get
            {
                return this["Usuario"] as string;
            }
        }

        [ConfigurationProperty("Senha", DefaultValue = "B5@dm99", IsRequired = false)]
        public string Senha
        {
            get
            {
                return this["Senha"] as string;
            }
        }


    }
}