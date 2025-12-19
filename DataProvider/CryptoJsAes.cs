using System.Security.Cryptography;
using System.Text;

namespace vynce_api.DataProvider
{
    public class CryptoJsAes
    {
        public static string Decrypt(string cipherTextBase64, string secretKey)
        {
            byte[] cipherBytes = Convert.FromBase64String(cipherTextBase64);

            // "Salted__" prefix (8 bytes) + salt (8 bytes)
            byte[] salt = cipherBytes.Skip(8).Take(8).ToArray();
            byte[] encrypted = cipherBytes.Skip(16).ToArray();

            // Derive key + IV (OpenSSL compatible)
            var keyIv = EVP_BytesToKey(
                Encoding.UTF8.GetBytes(secretKey),
                salt,
                32, // key size
                16  // IV size
            );

            using var aes = Aes.Create();
            aes.KeySize = 256;
            aes.BlockSize = 128;
            aes.Mode = CipherMode.CBC;
            aes.Padding = PaddingMode.PKCS7;
            aes.Key = keyIv.Key;
            aes.IV = keyIv.IV;

            using var decryptor = aes.CreateDecryptor();
            using var ms = new MemoryStream(encrypted);
            using var cs = new CryptoStream(ms, decryptor, CryptoStreamMode.Read);
            using var sr = new StreamReader(cs, Encoding.UTF8);

            return sr.ReadToEnd();
        }

        private static (byte[] Key, byte[] IV) EVP_BytesToKey(
            byte[] password,
            byte[] salt,
            int keySize,
            int ivSize
        )
        {
            using var md5 = MD5.Create();
            byte[] keyIv = new byte[keySize + ivSize];
            byte[] prev = Array.Empty<byte>();
            int offset = 0;

            while (offset < keyIv.Length)
            {
                byte[] input = prev
                    .Concat(password)
                    .Concat(salt)
                    .ToArray();

                prev = md5.ComputeHash(input);
                Buffer.BlockCopy(prev, 0, keyIv, offset, prev.Length);
                offset += prev.Length;
            }

            byte[] key = keyIv.Take(keySize).ToArray();
            byte[] iv = keyIv.Skip(keySize).Take(ivSize).ToArray();

            return (key, iv);
        }
    }
}
