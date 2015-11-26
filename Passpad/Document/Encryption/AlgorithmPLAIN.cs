namespace Passpad.Encryption
{
	class AlgorithmPlain : AbstractEncryptionAlgorithm
	{
		protected override byte[] EncodeBytes(byte[] data, string password)
		{
			return data;
		}

		protected override byte[] DecodeBytes(byte[] data, string password)
		{
			return data;
		}
	}
}
