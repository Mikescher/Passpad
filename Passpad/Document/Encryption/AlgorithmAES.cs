using System.IO;
using System.Linq;
using System.Security;
using System.Security.Cryptography;

namespace Passpad.Encryption
{
	class AlgorithmAES : AbstractEncryptionAlgorithm
	{
		private const int IV_SIZE = 16;
		private const int KEY_SIZE = 32;

		protected override byte[] EncodeBytes(byte[] data, SecureString password)
		{
			using (var aes = new AesCryptoServiceProvider())
			{
				var key = HashPassword(password, KEY_SIZE);
				var iv = GenerateSalt(IV_SIZE);

				aes.Mode = CipherMode.CBC;
				aes.Key = key;
				aes.IV = iv;

				var encryptor = aes.CreateEncryptor(key, iv);
				using (var msEncrypt = new MemoryStream())
				{
					using (var csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
					using (var swEncrypt = new BinaryWriter(csEncrypt))
					{
						swEncrypt.Write(data);
					}

					return Concat(iv, msEncrypt.ToArray());
				}
			}
		}

		protected override byte[] DecodeBytes(byte[] data, SecureString password)
		{
			using (var aes = new AesCryptoServiceProvider())
			{
				var key = HashPassword(password, KEY_SIZE);
				var iv = data.Take(IV_SIZE).ToArray();

				aes.Mode = CipherMode.CBC;
				aes.Key = key;
				aes.IV = iv;
				
				var decryptor = aes.CreateDecryptor(key, iv);
				using (var msDecrypt = new MemoryStream(data.Skip(IV_SIZE).ToArray()))
				using (var csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
				{
					return ReadToEnd(csDecrypt);
				}

			}
		}
	}
}
