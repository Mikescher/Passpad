using System;
using System.IO;
using System.Security;
using System.Text;
using System.Xml.Linq;

namespace Passpad.Document.Encryption
{
	static class EncryptionFileIO
	{
		public static XDocument ReadFile(string file)
		{
			return XDocument.Parse("<data>" + File.ReadAllText(file, Encoding.UTF8) + "</data>");
		}

		public static string ReadFile(XDocument xdoc, SecureString password, out EncryptionAlgorithm algorithm)
		{
			if (!Enum.TryParse(xdoc.Root?.Element("encrypted")?.Attribute("algorithm").Value ?? string.Empty, true, out algorithm))
			{
				algorithm = EncryptionAlgorithm.Plain;
				return null;
			}

			var data = (xdoc.Root?.Element("encrypted")?.Value ?? string.Empty)
				.Replace("\t", "")
				.Replace(" ", "")
				.Replace("\r", "")
				.Replace("\n", "")
				.Trim();

			var bdata = Convert.FromBase64String(data);

			return AbstractEncryptionAlgorithm.GetAlgorithm(algorithm).Decode(bdata, password);
		}

		public static string ReadPasswordHint(XDocument xdoc)
		{
			return xdoc.Root?.Element("hint")?.Value ?? string.Empty;
		}

		public static void SaveFile(string file, string text, string hint, SecureString password, EncryptionAlgorithm algorithm)
		{
			var data = AbstractEncryptionAlgorithm.GetAlgorithm(algorithm).Encode(text, password);
			var data64 = Convert.ToBase64String(data);

			StringBuilder result = new StringBuilder();

			result.AppendLine(string.Format("<hint>{0}</hint>", SecurityElement.Escape(hint)));
			result.AppendLine(string.Format("<encrypted algorithm=\"{0}\">", algorithm));
			for (int i = 0; i < Math.Ceiling(data64.Length / 64.0); i++)
			{
				result.AppendLine("    " + data64.Substring(i*64, Math.Min(64, data64.Length - i * 64)));
			}
			result.AppendLine("</encrypted>");

			File.WriteAllText(file, result.ToString(), Encoding.UTF8);
		}
	}
}
