using Microsoft.Win32;
using Passpad.Document.Encryption;
using Passpad.WPF.BaseViewModel;
using System;
using System.Security;
using System.Windows;
using System.Xml.Linq;
using Avalonia.Controls;
using PassPad;
using PassPad.Util;

namespace Passpad.Document
{
	public class PasspadDocument : ObservableObject
	{
		private EncryptionAlgorithm fileAlgorithm = EncryptionAlgorithm.Plain;
		private EncryptionAlgorithm _algorithm = EncryptionAlgorithm.Plain;
		public EncryptionAlgorithm Algorithm
		{
			get { return _algorithm; }
			set { _algorithm = value; RaisePropertyChanged(); RaisePropertyChanged(nameof(IsChanged)); RaisePropertyChanged(nameof(WindowTitle)); }
		}

		private SecureString filePassword = null;
		private SecureString _password = null;
		public SecureString Password
		{
			get { return _password; }
			set { _password = value; RaisePropertyChanged(); RaisePropertyChanged(nameof(IsChanged)); RaisePropertyChanged(nameof(WindowTitle)); }
		}

		private string fileContent = string.Empty;
		private string _content = string.Empty;
		public string Content
		{
			get { return _content; }
			set { _content = value; RaisePropertyChanged(); RaisePropertyChanged(nameof(IsChanged)); RaisePropertyChanged(nameof(WindowTitle)); }
		}

		private string fileHint = string.Empty;
		private string _hint = string.Empty;
		public string Hint
		{
			get { return _hint; }
			set { _hint = value; RaisePropertyChanged(); RaisePropertyChanged(nameof(IsChanged)); RaisePropertyChanged(nameof(WindowTitle)); }
		}

		private string _file = null;
		public string File
		{
			get { return _file; }
			set { _file = value; RaisePropertyChanged(); RaisePropertyChanged(nameof(IsChanged)); RaisePropertyChanged(nameof(WindowTitle)); }
		}

		public bool IsChanged => fileContent != Content || fileHint != Hint || filePassword != Password || fileAlgorithm != Algorithm;

		public string WindowTitle => string.Format("{0}{1} - Passpad v{2}", IsChanged ? "*" : "", File ?? "new", App.VERSION);

		private PasspadDocument(EncryptionAlgorithm algo, SecureString pw, string content, string hint, string file)
		{
			fileAlgorithm = algo;
			Algorithm = algo;

			filePassword = pw;
			Password = pw;

			fileContent = content;
			Content = content;

			fileHint = hint;
			Hint = hint;

			File = file;
		}

		public bool SaveDocument(Window owner)
		{
			if (File == null) return SaveDocumentAs(owner);

			try
			{
				while (Password == null || Password.Length == 0)
				{
					//TODO
					//var dialog = new ChangePasswordDialog();
//
					//if (dialog.ShowDialog(owner, false))
					//{
					//	Password = dialog.Password;
					//}
				}

				EncryptionFileIO.SaveFile(File, Content, Hint, Password, Algorithm);

				fileContent = Content;
				filePassword = Password;
				fileHint = Hint;
				fileAlgorithm = Algorithm;

				RaisePropertyChanged(nameof(IsChanged));
				RaisePropertyChanged(nameof(WindowTitle));
			}
			catch (Exception e)
			{
				MessageBox.Show("Error saving file", "Write error", MessageBoxButton.OK, MessageBoxImage.Error);
				MessageBox.Show(e.ToString(), "Write error", MessageBoxButton.OK, MessageBoxImage.Error);
				return false;
			}

			return true;
		}

		public bool SaveDocumentAs(Window owner)
		{
			//TODO
			//var sfd = new SaveFileDialog { Filter = "Encrypted Textfile (*.crypt.txt)|*.crypt.txt|All Files|*" };
//
			//if (sfd.ShowDialog() ?? false)
			//{
			//	File = sfd.FileName;
//
			//	return SaveDocument(owner);
			//}
//
			return false;
		}

		public static PasspadDocument NewEmpty()
		{
			return new PasspadDocument(EncryptionAlgorithm.AES, null, string.Empty, string.Empty, null);
		}

		public static PasspadDocument LoadDocument(Window owner, string filepath, SecureString initialPassword = null)
		{
			string hint;
			XDocument xdoc;
			try
			{
				xdoc = EncryptionFileIO.ReadFile(filepath);
				hint = EncryptionFileIO.ReadPasswordHint(xdoc);
			}
			catch (Exception e)
			{
				MessageBox.Show("Encrypted file had an unexpected format", "Read error", MessageBoxButton.OK, MessageBoxImage.Error);
				MessageBox.Show(e.ToString(), "Read error", MessageBoxButton.OK, MessageBoxImage.Error);
				
				return null;
			}

			var pass = initialPassword;
			for (;;)
			{
				if (pass == null)
				{
					//TODO
					//pass = PasswordDialog.ShowDialog(owner, hint);
					//if (pass == null) return null;
				}

				try
				{
					EncryptionAlgorithm alg;
					var content = EncryptionFileIO.ReadFile(xdoc, pass, out alg);

					return new PasspadDocument(alg, pass, content, hint, filepath);
				}
				catch (PasswordHashMismatchException)
				{
					MessageBox.Show("Encrypted data cannot be verified (wrong password ?)", "Read error", MessageBoxButton.OK, MessageBoxImage.Error);

					pass = null;
				}
				catch (Exception e)
				{
					MessageBox.Show("Can't read encrypted file (wrong file ?)" + Environment.NewLine + e.GetType().Name, "Read error", MessageBoxButton.OK, MessageBoxImage.Error);

					pass = null;
				}
			}
		}
	}
}
