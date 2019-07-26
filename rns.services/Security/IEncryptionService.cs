using System;
using System.Collections.Generic;
using System.Text;

namespace Rns.Services.Security
{
    public interface IEncryptionService
    {
        string CreateSaltKey(int size);

        string CreatePasswordHash(string password, string saltkey, string passwordFormat= "SHA512");

        string CreateHash(byte[] data, string hashAlgorithm);

        string EncryptText(string plainText, string encryptionPrivateKey = "");

        string DecryptText(string cipherText, string encryptionPrivateKey = "");

    }
}
