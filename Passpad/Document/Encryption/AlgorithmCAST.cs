using System.Security;
using Aced.Cryptography;

namespace Passpad.Document.Encryption
{
	class AlgorithmCAST : AbstractEncryptionAlgorithm
	{
		private const int KEY_SIZE = 56;

		protected override byte[] EncodeBytes(byte[] data, SecureString password)
		{
			var key = HashPassword(password, KEY_SIZE);

			var scheduledKey = AcedCast5.ScheduleKey(key);
			var iv = AcedCast5.GetOrdinaryIV(scheduledKey);

			byte[] result = (byte[])MakeMultipleLength(data, 8).Clone();

			AcedCast5.EncryptCBC(scheduledKey, result, 0, result.Length, iv);
			AcedCast5.ClearKey(scheduledKey);

			return result;
		}

		protected override byte[] DecodeBytes(byte[] data, SecureString password)
		{
			var key = HashPassword(password, KEY_SIZE);
			
			int[] scheduledKey = AcedCast5.ScheduleKey(key);
			long iv = AcedCast5.GetOrdinaryIV(scheduledKey);
			byte[] result = (byte[])data.Clone();
			AcedCast5.DecryptCBC(scheduledKey, result, 0, result.Length, iv);
			AcedCast5.ClearKey(scheduledKey);

			return TrimRightNull(result);
		}
	}
}
