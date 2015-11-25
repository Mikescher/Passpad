using System;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace Passpad.Encryption
{
    abstract class AbstractEncryptionAlgorithm
    {
	    private readonly byte[] salt = {
			0xEF, 0x03, 0x33, 0xC4, 0xEB, 0x4A, 0x06, 0x51,
			0x01, 0x17, 0xF8, 0x2E, 0xB4, 0x28, 0x60, 0x33,
			0x06, 0x1E, 0xBC, 0xF2, 0x38, 0x36, 0x62, 0x27,
			0x24, 0x65, 0x72, 0x06, 0xFE, 0xAD, 0x9C, 0xB6,
		};

        private const int PBKDF_ROUNDS = 40000;

		public abstract byte[] EncodeBytes(byte[] data, string password);
	    public abstract byte[] DecodeBytes(byte[] data, string password);

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

	    protected byte[] ComputeHash(byte[] data)
	    {
		    using (var sha = SHA256.Create()) return sha.ComputeHash(data);
	    }

	    protected byte[] HashPassword(string password, int size)
		{
			using (var rfc2898 = new Rfc2898DeriveBytes(password, salt, PBKDF_ROUNDS))
			{
				return rfc2898.GetBytes(size);
			}
		}

		public static byte[] EncodeText(string text)
		{
			return Encoding.UTF8.GetBytes(text);
		}

		public static string DecodeText(byte[] data)
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
    }
}
