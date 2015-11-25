namespace Passpad.Encryption
{
	class AlgorithmPlain : AbstractEncryptionAlgorithm
	{
		public override byte[] EncodeBytes(byte[] data, string password)
		{
			return data;
		}

		public override byte[] DecodeBytes(byte[] data, string password)
		{
			return data;
		}
	}
}
