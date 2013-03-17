using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;

namespace BsBios.Portal.UI.Configuration
{
    public class EmailDoPortal : ConfigurationSection
    {
        [ConfigurationProperty("Remetente", DefaultValue = "compras@bsbios.com", IsRequired = false)]
        public string Remetente
        {
            get
            {
                return this["Remetente"] as string;
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