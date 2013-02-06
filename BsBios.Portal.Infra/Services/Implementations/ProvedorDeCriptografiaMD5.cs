using System;
using System.Security.Cryptography;
using System.Text;
using BsBios.Portal.Infra.Services.Contracts;

namespace BsBios.Portal.Infra.Services.Implementations
{
    public class ProvedorDeCriptografiaMd5: IProvedorDeCriptografia
    {
        public string Criptografar(string dado)
        {
            //var provider = new MD5CryptoServiceProvider();
            //byte[] data = Encoding.ASCII.GetBytes(dado);
            //data = provider.ComputeHash(data);
            //String md5Hash = Encoding.ASCII.GetString(data);
            //return md5Hash;

            string hash = Convert.ToBase64String(
                MD5.Create().ComputeHash(Encoding.UTF8.GetBytes(dado))
            );

            return hash;
        }
    }
}
