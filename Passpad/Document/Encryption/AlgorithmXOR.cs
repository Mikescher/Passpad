using System.IO;
using System.Security;
using System.Security.Cryptography;
using TwoFishImplementation;

namespace Passpad.Document.Encryption
{
	class AlgorithmXOR : AbstractEncryptionAlgorithm
	{
		private const int KEY_SIZE = 16;
		private readonly byte[] IV = {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0};

		protected override byte[] EncodeBytes(byte[] data, SecureString password)
		{
			using (var algo = new XOR())
			{
				var key = HashPassword(password, KEY_SIZE);

				algo.Key = key;

				var encryptor = algo.CreateEncryptor(key, IV);
				using (var msEncrypt = new MemoryStream())
				{
					using (var csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
					using (var swEncrypt = new BinaryWriter(csEncrypt))
					{
						swEncrypt.Write(data);
					}

					return msEncrypt.ToArray();
				}
			}
		}

		protected override byte[] DecodeBytes(byte[] data, SecureString password)
		{
			using (var algo = new XOR())
			{
				var key = HashPassword(password, KEY_SIZE);

				algo.Key = key;

				var decryptor = algo.CreateDecryptor(key, IV);
				using (var msDecrypt = new MemoryStream(data))
				using (var csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
				{
					return TrimRightNull(ReadToEnd(csDecrypt));
				}

			}
		}
	}
}
