﻿using System.Security;

namespace Passpad.Document.Encryption
{
	class AlgorithmPlain : AbstractEncryptionAlgorithm
	{
		protected override byte[] EncodeBytes(byte[] data, SecureString password)
		{
			return data;
		}

		protected override byte[] DecodeBytes(byte[] data, SecureString password)
		{
			return data;
		}
	}
}
