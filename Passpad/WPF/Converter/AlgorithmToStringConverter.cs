﻿using System;
using System.Globalization;
using System.Windows.Data;
using Passpad.Encryption;

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
				case EncryptionAlgorithm.AES: return "AES";
				case EncryptionAlgorithm.TripleDES: return "TRIPLE DES";
				case EncryptionAlgorithm.CAST: return "CAST";
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
