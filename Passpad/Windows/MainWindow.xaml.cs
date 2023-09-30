using System;
using System.ComponentModel;
using System.IO;
using System.Windows;
using System.Windows.Input;

namespace Passpad.Windows
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow
	{
		private readonly MainObservableObject viewmodel;

		public MainWindow()
		{
			viewmodel = new MainObservableObject(this);
			this.DataContext = viewmodel;

			InitializeComponent();

			Editor.Options.EnableHyperlinks = false;
			Editor.Options.EnableEmailHyperlinks = false;
			Editor.Options.EnableRectangularSelection = true;
		}

		private void Command_New_OnExecuted(object sender, ExecutedRoutedEventArgs e)
		{
			viewmodel.NewDocument(this);
		}

		private void Command_Open_OnExecuted(object sender, ExecutedRoutedEventArgs e)
		{
			viewmodel.LoadDocument();
		}

		private void Command_Save_OnExecuted(object sender, ExecutedRoutedEventArgs e)
		{
			viewmodel.Document.SaveDocument(this);
		}

		private void Command_Export_OnExecuted(object sender, ExecutedRoutedEventArgs e)
		{
			viewmodel.ExportDocument();
		}

		private void Command_SaveAs_OnExecuted(object sender, ExecutedRoutedEventArgs e)
		{
			viewmodel.Document.SaveDocumentAs(this);
		}

		private void Command_Exit_OnExecuted(object sender, ExecutedRoutedEventArgs e)
		{
			if (viewmodel.Document.IsChanged)
			{
				if (MessageBox.Show("You have un saved changes.Would you like to save your document?", "Save Your Changes?", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
				{
					if (!viewmodel.Document.SaveDocument(this)) return;
				}
			}

			Close();
		}

		private void MainWindow_OnClosing(object sender, CancelEventArgs e)
		{
			if (viewmodel.Document.IsChanged)
			{
				var mbresult = MessageBox.Show("You have un saved changes.Would you like to save your document?", "Save Your Changes?", MessageBoxButton.YesNoCancel);
				if (mbresult == MessageBoxResult.Yes)
				{
					if (!viewmodel.Document.SaveDocument(this))
					{
						e.Cancel = true;
					}
				}
				else if (mbresult == MessageBoxResult.Cancel)
				{
					e.Cancel = true;
				}
			}
		}

		private void Command_Reload_OnExecuted(object sender, ExecutedRoutedEventArgs e)
		{
			viewmodel.ReloadDocument(this);
		}

		private void MainWindow_OnLoaded(object sender, RoutedEventArgs e)
		{
			var args = Environment.GetCommandLineArgs();

			if (args.Length >= 2 && File.Exists(args[1]))
			{
				viewmodel.LoadDocument(args[1]);
			}
		}

		private void MenuItem_View_Normal_OnClick(object sender, RoutedEventArgs e)
		{
			viewmodel.Theme = Theme.Normal;
		}

		private void MenuItem_View_Invisible_OnClick(object sender, RoutedEventArgs e)
		{
			viewmodel.Theme = Theme.Invisible;
		}

		private void MenuItem_View_LowContrastDark_OnClick(object sender, RoutedEventArgs e)
		{
			viewmodel.Theme = Theme.LowContrastDark;
		}

		private void MenuItem_View_LowContrastLight_OnClick(object sender, RoutedEventArgs e)
		{
			viewmodel.Theme = Theme.LowContrastLight;
		}

		private void MenuItem_Settings_Password_OnClick(object sender, RoutedEventArgs e)
		{
			viewmodel.ChangePassword(this);
		}

		private void MenuItem_Settings_Algorithm_OnClick(object sender, RoutedEventArgs e)
		{
			viewmodel.ChangeAlgorithm(this);
		}

		private void MenuItem_Settings_Hint_OnClick(object sender, RoutedEventArgs e)
		{
			viewmodel.ChangeHint(this);
		}

		private void MenuItem_View_WordWrap_OnClick(object sender, RoutedEventArgs e)
		{
			viewmodel.WordWrap = !viewmodel.WordWrap;
		}

		private void Command_Help_OnExecuted(object sender, ExecutedRoutedEventArgs e)
		{
			new AboutWindow {Owner = this}.ShowDialog();
		}

		private void UIElement_OnMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
		{
			viewmodel.ChangeAlgorithm(this);
		}
	}
}