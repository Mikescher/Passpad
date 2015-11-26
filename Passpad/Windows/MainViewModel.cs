using System;
using System.Text;
using System.Windows;
using System.Windows.Media;
using Microsoft.Win32;
using Passpad.BaseViewModel;
using Passpad.Dialogs;
using Passpad.Document;

namespace Passpad
{
    class MainObservableObject : ObservableObject
    {
	    private PasspadDocument _document;
	    public PasspadDocument Document
	    {
		    get { return _document; }
			set { _document = value; RaisePropertyChanged(); }
	    }

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

		private readonly Window Owner;

		public MainObservableObject(Window owner)
		{
			Owner = owner;

			Document = PasspadDocument.NewEmpty();
		}

		public void NewDocument()
	    {
		    if (Document.IsChanged)
		    {
			    if (MessageBox.Show("You have un saved changes.Would you like to save your document?", "Save Your Changes?", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
			    {
				    if (!Document.SaveDocument()) return;
			    }
		    }

			Document = PasspadDocument.NewEmpty();
	    }

	    public void LoadDocument()
	    {
		    var ofd = new OpenFileDialog {Filter = "Encrypted Textfile (*.crypt.txt)|*.crypt.txt|All Files|*"};

		    if (ofd.ShowDialog() ?? false)
		    {
			    LoadDocument(ofd.FileName);
		    }
		}

		public void LoadDocument(string file)
		{
			var newdoc = PasspadDocument.LoadDocument(Owner, file);
			if (newdoc != null) Document = newdoc;
		}

		public void ReloadDocument()
	    {
		    if (Document.File == null)
		    {
			    LoadDocument();
		    }
		    else
		    {
				var newdoc = PasspadDocument.LoadDocument(Owner, Document.File, Document.Password);
				if (newdoc != null) Document = newdoc;
		    }
		}

	    public void ExportDocument()
	    {
		    var sfd = new SaveFileDialog {Filter = "Textfile (*.txt)|*.txt|All Files|*"};

		    if (sfd.ShowDialog() ?? false)
		    {
			    System.IO.File.WriteAllText(sfd.FileName, Document.Content, Encoding.UTF8);
		    }
		}

		public void ChangeHint(Window owner)
		{
			var dialog = new ChangeHintDialog();

			if (dialog.ShowDialog(owner, Document.Hint))
			{
				Document.Hint = dialog.Hint;
			}
		}

		public void ChangePassword(Window owner)
		{
			var dialog = new ChangePasswordDialog();

			if (dialog.ShowDialog(owner, Document.Password))
			{
				Document.Password = dialog.Password;
			}
		}

		public void ChangeAlgorithm(Window owner)
		{
			var dialog = new ChangeAlgorithmDialog();

			if (dialog.ShowDialog(owner, Document.Algorithm))
			{
				Document.Algorithm = dialog.Algorithm;
			}
		}
	}
}
