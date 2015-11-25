using BlowFishCS;

namespace Passpad.Encryption
{
	class AlgorithmBlowfish : AbstractEncryptionAlgorithm
	{
		private const int KEY_SIZE = 56;

		protected override byte[] EncodeBytes(byte[] data, string password)
		{
			var key = HashPassword(password, KEY_SIZE);

			var fish = new BlowFish(key);

			return fish.Encrypt_CBC(data);
		}

		protected override byte[] DecodeBytes(byte[] data, string password)
		{
			var key = HashPassword(password, KEY_SIZE);

			var fish = new BlowFish(key);

			return fish.Decrypt_CBC(data);
		}
	}
}
