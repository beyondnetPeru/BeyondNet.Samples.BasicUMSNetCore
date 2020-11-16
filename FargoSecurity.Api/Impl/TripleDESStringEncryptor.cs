using System.IO;
using System.Security.Cryptography;
using System.Text;
using Microsoft.Extensions.Configuration;

namespace FargoSecurity.Api.Impl
{
    public class TripleDesStringEncryptor 
    {
        private readonly IConfiguration _configuration;

        private readonly byte[] _key;

        private readonly byte[] _iv;

        private readonly TripleDESCryptoServiceProvider _provider;

        public TripleDesStringEncryptor(IConfiguration configuration)
        {
            _configuration = configuration;
            
            _key = Encoding.ASCII.GetBytes(configuration.GetValue<string>("Encrypt:Key"));
            
            _iv = Encoding.ASCII.GetBytes(configuration.GetValue<string>("Encrypt:Iv"));

            _provider = new TripleDESCryptoServiceProvider();
        }

        #region IStringEncryptor Members

        public string Encrypt(string plainText)
        {
            return Transform(plainText, _provider.CreateEncryptor(_key, _iv));
        }

        public string Decrypt(string encryptedText)
        {
            return Transform(encryptedText, _provider.CreateDecryptor(_key, _iv));
        }

        #endregion

        private string Transform(string text, ICryptoTransform transform)
        {
            if (text == null)
            {
                return null;
            }

            using MemoryStream stream = new MemoryStream();

            using CryptoStream cryptoStream = new CryptoStream(stream, transform, CryptoStreamMode.Write);

            byte[] input = Encoding.Default.GetBytes(text);

            cryptoStream.Write(input, 0, input.Length);

            cryptoStream.FlushFinalBlock();

            return Encoding.Default.GetString(stream.ToArray());
        }
    }
}
