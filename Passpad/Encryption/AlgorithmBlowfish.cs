using BlowFishCS;

namespace Passpad.Encryption
{
	class AlgorithmBlowfish : AbstractEncryptionAlgorithm
	{
		private const int KEY_SIZE = 56;

		public override byte[] EncodeBytes(byte[] data, string password)
		{
			var key = HashPassword(password, KEY_SIZE);

			var fish = new BlowFish(key);

			return fish.Encrypt_CBC(data);
		}

		public override byte[] DecodeBytes(byte[] data, string password)
		{
			var key = HashPassword(password, KEY_SIZE);

			var fish = new BlowFish(key);

			return fish.Decrypt_CBC(data);
		}
	}
}
