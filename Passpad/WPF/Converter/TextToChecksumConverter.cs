using System;
using System.Globalization;
using System.Security.Cryptography;
using System.Text;
using System.Windows.Data;

namespace Passpad.WPF.Converter
{
	class TextToChecksumConverter : IValueConverter
	{
		private static readonly HashAlgorithm _checksum = MD5.Create();

		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			return ByteToHexBitFiddle(_checksum.ComputeHash(Encoding.UTF8.GetBytes(value.ToString())));
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			throw new NotSupportedException();
		}

		// http://stackoverflow.com/a/14333437/1761622
		private static string ByteToHexBitFiddle(byte[] bytes)
		{
			char[] c = new char[bytes.Length * 2];
			for (int i = 0; i < bytes.Length; i++)
			{
				var b = bytes[i] >> 4;
				c[i * 2] = (char)(55 + b + (((b - 10) >> 31) & -7));
				b = bytes[i] & 0xF;
				c[i * 2 + 1] = (char)(55 + b + (((b - 10) >> 31) & -7));
			}
			return new string(c);
		}
	}
}
