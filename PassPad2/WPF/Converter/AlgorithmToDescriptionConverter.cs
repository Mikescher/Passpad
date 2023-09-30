using System;
using System.Globalization;
using Avalonia.Data.Converters;
using Passpad.Document.Encryption;

namespace Passpad.WPF.Converter
{
	class AlgorithmToDescriptionConverter : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			switch ((EncryptionAlgorithm)value)
			{
				case EncryptionAlgorithm.Plain:
					return "Normal text file";
				case EncryptionAlgorithm.Blowfish:
				case EncryptionAlgorithm.Twofish:
				case EncryptionAlgorithm.AES:
				case EncryptionAlgorithm.DES:
				case EncryptionAlgorithm.TripleDES:
				case EncryptionAlgorithm.CAST:
					return "Encrypted text file";
				case EncryptionAlgorithm.XOR:
					return "Obfuscated text file";
				default:
					throw new ArgumentOutOfRangeException(nameof(value), value, null);
			}
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			throw new NotSupportedException();
		}
	}
}
