using System;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Input;

namespace Passpad
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		private MainViewModel Viewmodel;

		public MainWindow()
		{
			Viewmodel = new MainViewModel(this);
            this.DataContext = Viewmodel;

			InitializeComponent();

			Editor.Options.EnableHyperlinks = false;
			Editor.Options.EnableEmailHyperlinks = false;
			Editor.Options.EnableRectangularSelection = true;
		}

		private void Command_New_OnExecuted(object sender, ExecutedRoutedEventArgs e)
		{
			Viewmodel.NewDocument();
		}

		private void Command_Open_OnExecuted(object sender, ExecutedRoutedEventArgs e)
		{
			Viewmodel.LoadDocument();
		}

		private void Command_Save_OnExecuted(object sender, ExecutedRoutedEventArgs e)
		{
			Viewmodel.SaveDocument();
		}

		private void Command_Export_OnExecuted(object sender, ExecutedRoutedEventArgs e)
		{
			Viewmodel.ExportDocument();
		}

		private void Command_SaveAs_OnExecuted(object sender, ExecutedRoutedEventArgs e)
		{
			Viewmodel.SaveDocumentAs();
		}

		private void Command_Exit_OnExecuted(object sender, ExecutedRoutedEventArgs e)
		{
			if (Viewmodel.IsChanged)
			{
				if (MessageBox.Show("You have un saved changes.Would you like to save your document?", "Save Your Changes?", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
				{
					if (!Viewmodel.SaveDocument()) return;
				}
			}

			Close();
		}

		private void Command_Reload_OnExecuted(object sender, ExecutedRoutedEventArgs e)
		{
			Viewmodel.ReloadDocument();
		}

		private void MainWindow_OnLoaded(object sender, RoutedEventArgs e)
		{
			var args = Environment.GetCommandLineArgs();

			if (args.Length >= 2 && File.Exists(args[1]))
			{
				Viewmodel.File = args[1];
				Viewmodel.ReloadDocument();
			}
		}

		private void MenuItem_View_Normal_OnClick(object sender, RoutedEventArgs e)
		{
			Viewmodel.Theme = Theme.Normal;
		}

		private void MenuItem_View_Invisible_OnClick(object sender, RoutedEventArgs e)
		{
			Viewmodel.Theme = Theme.Invisible;
		}

		private void MenuItem_View_LowContrastDark_OnClick(object sender, RoutedEventArgs e)
		{
			Viewmodel.Theme = Theme.LowContrastDark;
		}

		private void MenuItem_View_LowContrastLight_OnClick(object sender, RoutedEventArgs e)
		{
			Viewmodel.Theme = Theme.LowContrastLight;
		}

		private void MenuItem_Settings_Password_OnClick(object sender, RoutedEventArgs e)
		{
			Viewmodel.ChangePassword(this);
		}

		private void MenuItem_Settings_Algorithm_OnClick(object sender, RoutedEventArgs e)
		{
			Viewmodel.ChangeAlgorithm(this);
		}

		private void MenuItem_Settings_Hint_OnClick(object sender, RoutedEventArgs e)
		{
			Viewmodel.ChangeHint(this);
		}

		private void MenuItem_View_WordWrap_OnClick(object sender, RoutedEventArgs e)
		{
			Viewmodel.WordWrap = !Viewmodel.WordWrap;
		}

		private void Command_Help_OnExecuted(object sender, ExecutedRoutedEventArgs e)
		{
			new AboutWindow {Owner = this}.ShowDialog();
		}
	}
}