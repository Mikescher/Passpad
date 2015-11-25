using System.IO;
using System.Linq;
using System.Security.Cryptography;
using TwoFishImplementation;

namespace Passpad.Encryption
{
	class AlgorithmTwofish : AbstractEncryptionAlgorithm
	{
		private const int IV_SIZE = 16;
		private const int KEY_SIZE = 32;

		public override byte[] EncodeBytes(byte[] data, string password)
		{
			using (var fish = new Twofish())
			{
				var key = HashPassword(password, KEY_SIZE);
				var iv = GenerateSalt(IV_SIZE);

				fish.Key = key;
				fish.IV = iv;

				var encryptor = fish.CreateEncryptor(key, iv);
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

		public override byte[] DecodeBytes(byte[] data, string password)
		{
			using (var fish = new Twofish())
			{
				var key = HashPassword(password, KEY_SIZE);
				var iv = data.Take(IV_SIZE).ToArray();

				fish.Key = key;
				fish.IV = iv;

				var decryptor = fish.CreateDecryptor(key, iv);
				using (var msDecrypt = new MemoryStream(data.Skip(IV_SIZE).ToArray()))
				using (var csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
				{
					return ReadToEnd(csDecrypt);
				}

			}
		}
	}
}
