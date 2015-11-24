using System;
using System.Windows;
using System.Windows.Media;
using Microsoft.Win32;
using Passpad.BaseViewModel;
using Passpad.Dialogs;
using Passpad.Encryption;

namespace Passpad
{
    class MainViewModel : ViewModel
    {
		private EncryptionAlgorithm fileAlgorithm = EncryptionAlgorithm.Plain;
		private EncryptionAlgorithm _algorithm = EncryptionAlgorithm.Plain;
		public EncryptionAlgorithm Algorithm
		{
			get { return _algorithm; }
			set { _algorithm = value; RaisePropertyChanged(); RaisePropertyChanged("IsChanged"); RaisePropertyChanged("Title"); }
		}

		private string filePassword = null;
	    private string _password = null;
	    public string Password
	    {
		    get { return _password; }
		    set { _password = value;  RaisePropertyChanged(); RaisePropertyChanged("IsChanged"); RaisePropertyChanged("Title"); }
		}

	    private string fileContent = string.Empty;
		private string _content = string.Empty;
		public string Content
		{
			get { return _content; }
			set { _content = value; RaisePropertyChanged(); RaisePropertyChanged("IsChanged"); RaisePropertyChanged("Title"); }
		}

		private string fileHint = string.Empty;
		private string _hint = string.Empty;
		public string Hint
		{
			get { return _hint; }
			set { _hint = value; RaisePropertyChanged(); RaisePropertyChanged("IsChanged"); RaisePropertyChanged("Title"); }
		}

		private string _file = null;
		public string File
		{
			get { return _file; }
			set { _file = value; RaisePropertyChanged(); RaisePropertyChanged("IsChanged"); RaisePropertyChanged("Title"); }
		}

	    public bool IsChanged => fileContent != Content || fileHint != Hint || filePassword != Password || fileAlgorithm != Algorithm || File == null;

	    public string Title => string.Format("{0}{1} - Passpad v{2}", IsChanged ? "*" : "", File ?? "new", App.VERSION);

		//###########################################################################

		private bool _wordwrap = true;
		public bool WordWrap
		{
			get { return _wordwrap; }
			set { _wordwrap = value; RaisePropertyChanged(); }
		}

	    private Theme _theme;
		public Theme Theme
		{
			get { return _theme; }
			set { _theme = value; RaisePropertyChanged(); RaisePropertyChanged("EditorForeground"); RaisePropertyChanged("EditorBackground"); }
		}

	    public Brush EditorForeground
	    {
		    get
		    {
			    switch (Theme)
			    {
				    case Theme.Normal: return Brushes.Black;
				    case Theme.Invisible: return Brushes.White;
					case Theme.LowContrastDark: return new SolidColorBrush(Color.FromRgb(128, 128, 128));
					case Theme.LowContrastLight: return new SolidColorBrush(Color.FromRgb(192, 192, 192));
					default: throw new ArgumentOutOfRangeException();
			    }
		    }
		}

		public Brush EditorBackground
		{
			get
			{
				switch (Theme)
				{
					case Theme.Normal: return Brushes.White;
					case Theme.Invisible: return Brushes.White;
					case Theme.LowContrastDark: return new SolidColorBrush(Color.FromRgb(105, 105, 105));
					case Theme.LowContrastLight: return new SolidColorBrush(Color.FromRgb(220, 220, 220));
					default: throw new ArgumentOutOfRangeException();
				}
			}
		}

		public void NewDocument()
	    {
		    if (IsChanged)
		    {
			    if (MessageBox.Show("You have un saved changes.Would you like to save your document?", "Save Your Changes?", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
			    {
				    if (!SaveDocument()) return;
			    }
		    }

		    filePassword = null;
		    Password = null;

		    fileContent = string.Empty;
		    Content = string.Empty;

		    fileHint = string.Empty;
		    Hint = string.Empty;

		    File = null;
	    }

	    public void LoadDocument()
	    {
		    var ofd = new OpenFileDialog {Filter = "Encrypted Textfile (*.crypt.txt)|*.crypt.txt|All Files|*"};

		    if (ofd.ShowDialog() ?? false)
		    {
			    File = ofd.FileName;

			    ReloadDocument();
		    }
	    }

	    public void ReloadDocument()
	    {
		    if (File == null)
		    {
			    LoadDocument();
			    return;
		    }

		    fileContent = System.IO.File.ReadAllText(File);
		    filePassword = null;
		    fileHint = string.Empty;

		    Content = fileContent;
		    Password = filePassword;
		    Hint = fileHint;
	    }

	    public bool SaveDocument()
	    {
		    if (File == null) return SaveDocumentAs();

		    System.IO.File.WriteAllText(File, Content);

		    fileContent = Content;
		    filePassword = Password;
		    fileHint = Hint;

		    return true;
	    }

	    public bool SaveDocumentAs()
	    {
		    var sfd = new SaveFileDialog {Filter = "Encrypted Textfile (*.crypt.txt)|*.crypt.txt|All Files|*"};

		    if (sfd.ShowDialog() ?? false)
		    {
			    File = sfd.FileName;

			    return SaveDocument();
		    }

		    return false;
	    }

	    public void ExportDocument()
	    {
		    var sfd = new SaveFileDialog {Filter = "Textfile (*.txt)|*.txt|All Files|*"};

		    if (sfd.ShowDialog() ?? false)
		    {
			    System.IO.File.WriteAllText(sfd.FileName, Content);
		    }
		}

		public void ChangeHint(Window owner)
		{
			var dialog = new ChangeHintDialog();

			if (dialog.ShowDialog(owner, Hint))
			{
				Hint = dialog.Hint;
			}
		}

		public void ChangePassword(Window owner)
		{
			var dialog = new ChangePasswordDialog();

			if (dialog.ShowDialog(owner, Password))
			{
				Password = dialog.Password;
			}
		}

		public void ChangeAlgorithm(Window owner)
		{
			var dialog = new ChangeAlgorithmDialog();

			if (dialog.ShowDialog(owner, Algorithm))
			{
				Algorithm = dialog.Algorithm;
			}
		}
	}
}
