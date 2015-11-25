using System.IO;
using System.Security.Cryptography;
using TwoFishImplementation;

namespace Passpad.Encryption
{
	class AlgorithmXOR : AbstractEncryptionAlgorithm
	{
		private const int KEY_SIZE = 32;
		private readonly byte[] IV = {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0};

		public override byte[] EncodeBytes(byte[] data, string password)
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

		public override byte[] DecodeBytes(byte[] data, string password)
		{
			using (var fish = new Twofish())
			{
				var key = HashPassword(password, KEY_SIZE);

				fish.Key = key;

				var decryptor = fish.CreateDecryptor(key, IV);
				using (var msDecrypt = new MemoryStream(data))
				using (var csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
				{
					return TrimRightNull(ReadToEnd(csDecrypt));
				}

			}
		}
	}
}
