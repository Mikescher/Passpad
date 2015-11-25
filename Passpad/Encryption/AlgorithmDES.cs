using System.IO;
using System.Linq;
using System.Security.Cryptography;

namespace Passpad.Encryption
{
	class AlgorithmDES : AbstractEncryptionAlgorithm
	{
		private const int IV_SIZE = 8;
		private const int KEY_SIZE = 24;

		protected override byte[] EncodeBytes(byte[] data, string password)
		{
			using (var des = new DESCryptoServiceProvider())
			{
				var key = HashPassword(password, KEY_SIZE);
				var iv = GenerateSalt(IV_SIZE);

				des.Mode = CipherMode.CBC;
				des.Key = key;
				des.IV = iv;

				var encryptor = des.CreateEncryptor(key, iv);
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

		protected override byte[] DecodeBytes(byte[] data, string password)
		{
			using (var des = new DESCryptoServiceProvider())
			{
				var key = HashPassword(password, KEY_SIZE);
				var iv = data.Take(IV_SIZE).ToArray();

				des.Mode = CipherMode.CBC;
				des.Key = key;
				des.IV = iv;
				
				var decryptor = des.CreateDecryptor(key, iv);
				using (var msDecrypt = new MemoryStream(data.Skip(IV_SIZE).ToArray()))
				using (var csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
				{
					return ReadToEnd(csDecrypt);
				}

			}
		}
	}
}
