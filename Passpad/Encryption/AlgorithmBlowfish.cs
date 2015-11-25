﻿using System.Linq;
using BlowFishCS;

namespace Passpad.Encryption
{
	class AlgorithmBlowfish : AbstractEncryptionAlgorithm
	{
		private const int KEY_SIZE = 56;
		private const int IV_SIZE = 8;

		protected override byte[] EncodeBytes(byte[] data, string password)
		{
			var key = HashPassword(password, KEY_SIZE);
			var iv = GenerateSalt(IV_SIZE);

			var fish = new BlowFish(key) {IV = iv};
			
			return Concat(iv, fish.Encrypt_CBC(data));
		}

		protected override byte[] DecodeBytes(byte[] data, string password)
		{
			var key = HashPassword(password, KEY_SIZE);
			var iv = data.Take(IV_SIZE).ToArray();

			var fish = new BlowFish(key) { IV = iv };

			return TrimRightNull(fish.Decrypt_CBC(data.Skip(IV_SIZE).ToArray()));
		}
	}
}
