using System;
using System.Globalization;
using System.Security.Cryptography;
using BsBios.Portal.Infra.Services.Contracts;

namespace BsBios.Portal.Infra.Services.Implementations
{
    public class GeradorDeSenha:IGeradorDeSenha
    {
        public string Gerar(int numeroDeCaracteresAlfabeticos, int numeroDeCaracteresNumericos)
        {
            const string lowers = "abcdefghijklmnopqrstuvwxyz";
            const string number = "0123456789";

            var random = new Random();

            string generated = "!";
            for (int i = 1; i <= numeroDeCaracteresAlfabeticos; i++)
                generated = generated.Insert(
                    random.Next(generated.Length),
                    lowers[random.Next(lowers.Length - 1)].ToString(CultureInfo.InvariantCulture)
                );

            for (int i = 1; i <= numeroDeCaracteresNumericos; i++)
                generated = generated.Insert(
                    random.Next(generated.Length),
                    number[random.Next(number.Length - 1)].ToString(CultureInfo.InvariantCulture)
                );

            return generated.Replace("!", string.Empty);
        }

        public string GerarGuid(int tamanho)
        {
            var guid = Guid.NewGuid();
            return guid.ToString().Replace("-", "").Substring(0,tamanho);
        }
    }
}