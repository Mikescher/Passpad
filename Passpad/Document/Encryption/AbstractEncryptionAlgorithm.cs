using System;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security;
using System.Security.Cryptography;
using System.Text;

namespace Passpad.Document.Encryption
{
	abstract class AbstractEncryptionAlgorithm
	{
		private const int HASH_LENGTH = 32;

		private readonly byte[] salt = {
			0xEF, 0x03, 0x33, 0xC4, 0xEB, 0x4A, 0x06, 0x51,
			0x01, 0x17, 0xF8, 0x2E, 0xB4, 0x28, 0x60, 0x33,
			0x06, 0x1E, 0xBC, 0xF2, 0x38, 0x36, 0x62, 0x27,
			0x24, 0x65, 0x72, 0x06, 0xFE, 0xAD, 0x9C, 0xB6,
		};

		private const int PBKDF_ROUNDS = 40000;

		protected abstract byte[] EncodeBytes(byte[] data, SecureString password);
		protected abstract byte[] DecodeBytes(byte[] data, SecureString password);

		public byte[] Encode(string data, SecureString password)
		{
			var bdata = EncodeText(data);
			var cdata = EncodeBytes(bdata, password);

			return Concat(ComputeHash(bdata), cdata);
		}

		public string Decode(byte[] data, SecureString password)
		{
			var bdata = data.Skip(HASH_LENGTH).ToArray();
			var hash = data.Take(HASH_LENGTH).ToArray();

			var cdata = DecodeBytes(bdata, password);

			if (! hash.SequenceEqual(ComputeHash(cdata)))
			{
				throw new PasswordHashMismatchException("SHA-256 Hash mismatch");
			}

			return DecodeText(cdata);
		}

		public static AbstractEncryptionAlgorithm GetAlgorithm(EncryptionAlgorithm algo)
		{
			switch (algo)
			{
				case EncryptionAlgorithm.Plain:
					return new AlgorithmPlain();
				case EncryptionAlgorithm.Blowfish:
					return new AlgorithmBlowfish();
				case EncryptionAlgorithm.Twofish:
					return new AlgorithmTwofish();
				case EncryptionAlgorithm.AES:
					return new AlgorithmAES();
				case EncryptionAlgorithm.TripleDES:
					return new AlgorithmTripleDES();
				case EncryptionAlgorithm.CAST:
					return new AlgorithmCAST();
				case EncryptionAlgorithm.XOR:
					return new AlgorithmXOR();
				case EncryptionAlgorithm.DES:
					return new AlgorithmDES();
				default:
					throw new NotImplementedException();
			}
		}

		protected byte[] GenerateSalt(int length)
		{
			using (var rng = new RNGCryptoServiceProvider())
			{
				var result = new byte[length];
				rng.GetBytes(result);

				return result;
			}
		}

		private byte[] ComputeHash(byte[] data)
		{
			using (var sha = SHA256.Create()) return sha.ComputeHash(data);
		}

		protected byte[] HashPassword(SecureString password, int size)
		{
			IntPtr bstr = IntPtr.Zero;
			byte[] workArray = null;
			try
			{
				bstr = Marshal.SecureStringToBSTR(password);
				unsafe
				{
					byte* bstrBytes = (byte*)bstr;
					workArray = new byte[password.Length * 2];

					for (int i = 0; i < workArray.Length; i++)
						workArray[i] = *bstrBytes++;
				}

				using (var rfc2898 = new Rfc2898DeriveBytes(workArray, salt, PBKDF_ROUNDS))
				{
					return rfc2898.GetBytes(size);
				}
			}
			finally
			{
				if (workArray != null)
					for (int i = 0; i < workArray.Length; i++)
						workArray[i] = 0;
				if (bstr != IntPtr.Zero)
					Marshal.ZeroFreeBSTR(bstr);
			}
		}

		private byte[] EncodeText(string text)
		{
			return Encoding.UTF8.GetBytes(text);
		}

		private string DecodeText(byte[] data)
		{
			return Encoding.UTF8.GetString(data);
		}

		protected byte[] Concat(byte[] bytes0, params byte[][] bytes)
		{
			return bytes.Aggregate(bytes0, Concat);
		}

		protected byte[] Concat(byte[] first, byte[] second)
		{
			var ret = new byte[first.Length + second.Length];

			Buffer.BlockCopy(first, 0, ret, 0, first.Length);
			Buffer.BlockCopy(second, 0, ret, first.Length, second.Length);

			return ret;
		}

		protected byte[] ReadToEnd(Stream stream)
		{
			byte[] buffer = new byte[32768];
			using (MemoryStream ms = new MemoryStream())
			{
				for (;;)
				{
					int read = stream.Read(buffer, 0, buffer.Length);
					if (read <= 0)
						return ms.ToArray();
					ms.Write(buffer, 0, read);
				}
			}
		}
		

		protected byte[] TrimRightNull(byte[] data)
		{
			int len = data.Length;

			while (len > 0 && data[len - 1] == 0) len--;

			return data.Take(len).ToArray();
		}

		protected byte[] MakeMultipleLength(byte[] data, int div)
		{
			int add = (div - (data.Length % div)) % div;

			var rdata = data.AsEnumerable();

			for (int i = 0; i < add; i++)
				rdata = rdata.Concat(new byte[] {0});

			return rdata.ToArray();
		}
	}
}
