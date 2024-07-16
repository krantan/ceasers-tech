using System.Security.Cryptography;
using System.Text;

namespace api
{
    public static class CryptData
    {
        private static string PrivateKey
        {
            get
            {
                const string envVarName = "APP_SK";

                var envKeyValue = Environment.GetEnvironmentVariable(envVarName) ?? "d8e8fca2dc0f896fd7cb4cb0031ba249";

                if (string.IsNullOrEmpty(envKeyValue))
                {
                    throw new Exception("Invalid Encryption key");
                }

                return envKeyValue;
            }
        }

        private static string HashSalt
        {
            get
            {
                const string envVarName = "APP_HASH_SALT";

                var envKeyValue = Environment.GetEnvironmentVariable(envVarName) ?? "d8e8fca2dc0f896fd7cb4cb0031ba249";

                if (string.IsNullOrEmpty(envKeyValue))
                {
                    throw new Exception("Invalid Hash Salt");
                }

                return envKeyValue;
            }
        }

        public static string Decrypt(string payload, string salt)
        {
            if (string.IsNullOrEmpty(payload))
                throw new ArgumentNullException(nameof(payload));
            if (string.IsNullOrEmpty(PrivateKey))
                throw new ArgumentNullException(nameof(PrivateKey));
            if (string.IsNullOrEmpty(salt))
                throw new ArgumentNullException(nameof(salt));

            using var aesAlg = Aes.Create();
            aesAlg.Mode = CipherMode.CBC;
            aesAlg.Key = CreateAesKey(PrivateKey);
            aesAlg.IV = Convert.FromBase64String(salt);

            var decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);

            using var msDecrypt = new MemoryStream(Convert.FromBase64String(payload));
            using var csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read);
            using var srDecrypt = new StreamReader(csDecrypt);
            var plaintext = srDecrypt.ReadToEnd();

            return plaintext;
        }

        public static string Encrypt(string payload, string salt)
        {
            if (string.IsNullOrEmpty(payload))
                throw new ArgumentNullException(nameof(payload));
            if (string.IsNullOrEmpty(PrivateKey))
                throw new ArgumentNullException(nameof(PrivateKey));
            if (string.IsNullOrEmpty(salt))
                throw new ArgumentNullException(nameof(salt));

            byte[] encrypted;

            using (var aesAlg = Aes.Create())
            {
                aesAlg.Mode = CipherMode.CBC;
                aesAlg.Key = CreateAesKey(PrivateKey);
                aesAlg.IV = Convert.FromBase64String(salt);

                var encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV);

                using (var msEncrypt = new MemoryStream())
                {
                    using (var csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                    {
                        using (var swEncrypt = new StreamWriter(csEncrypt))
                        {
                            swEncrypt.Write(payload);
                        }
                        encrypted = msEncrypt.ToArray();
                    }
                }
            }

            return Convert.ToBase64String(encrypted);
        }

        private static byte[] CreateAesKey(string inputString)
        {
            return Encoding.UTF8.GetByteCount(inputString) == 32 ? Encoding.UTF8.GetBytes(inputString) : SHA256.Create().ComputeHash(Encoding.UTF8.GetBytes(inputString));
        }

        public static string GenerateSalt()
        {
            byte[] salt = new byte[16];
            salt = RandomNumberGenerator.GetBytes(salt.Length);
            return Convert.ToBase64String(salt);
        }

        public static string Hash(string payload, string salt)
        {
            const int keySize = 64;
            const int iterations = 350000;
            HashAlgorithmName hashAlgorithm = HashAlgorithmName.SHA512;

            var hashed = Rfc2898DeriveBytes.Pbkdf2(Convert.FromBase64String(payload), Convert.FromBase64String(salt), iterations, hashAlgorithm, keySize);
            return Convert.ToBase64String(hashed);
        }

        public static string HashQuick(string payload)
        {
            using var sha256 = SHA256.Create();
            var hashed = sha256.ComputeHash(Encoding.UTF8.GetBytes(payload));
            return Convert.ToBase64String(hashed);
        }
    }
}
