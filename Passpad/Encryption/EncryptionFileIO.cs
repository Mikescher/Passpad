using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml.Linq;
using Passpad.Encryption;

namespace Passpad
{
	class EncryptionFileIO
	{
		public static string ReadFile(string file, string password, out EncryptionAlgorithm algorithm)
		{
			var text = "<data>" + File.ReadAllText(file, Encoding.UTF8) + "</data>";
			var xdoc = XDocument.Parse(text);
				
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

		public static string ReadPasswordHint(string file)
		{
			var text = "<data>" + File.ReadAllText(file) + "</data>";
			var xdoc = XDocument.Parse(text);

			return xdoc.Root?.Element("hint")?.Value ?? string.Empty;
		}

		public static void SaveFile(string file, string text, string hint, string password, EncryptionAlgorithm algorithm)
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
