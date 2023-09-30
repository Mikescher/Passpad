using System;
using System.Globalization;
using Avalonia.Data.Converters;
using Passpad.Document.Encryption;

namespace Passpad.WPF.Converter
{
	class AlgorithmToStringConverter : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			switch ((EncryptionAlgorithm)value)
			{
				case EncryptionAlgorithm.Plain: return "PLAIN";
				case EncryptionAlgorithm.Blowfish: return "BLOWFISH";
				case EncryptionAlgorithm.Twofish: return "TWOFISH";
				case EncryptionAlgorithm.AES: return "AES-256";
				case EncryptionAlgorithm.TripleDES: return "TRIPLE DES";
				case EncryptionAlgorithm.DES: return "DES";
				case EncryptionAlgorithm.CAST: return "CAST-128";
				case EncryptionAlgorithm.XOR: return "XOR";
				default: throw new ArgumentOutOfRangeException(nameof(value), value, null);
			}
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			throw new NotSupportedException();
		}
	}
}
