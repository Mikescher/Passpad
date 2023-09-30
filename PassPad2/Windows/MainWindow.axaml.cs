using System;
using System.ComponentModel;
using System.IO;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using AvaloniaEdit;
using PassPad.Util;
using Passpad.Windows;
using ReactiveUI;

namespace PassPad.Windows;

public partial class MainWindow : Window
{
	private readonly MainObservableObject viewmodel;

	public MainWindow()
	{
		viewmodel = new MainObservableObject(this);
		this.DataContext = viewmodel;

		InitializeComponent();

		//TODO
		//Editor.Options.EnableHyperlinks = false;
		//Editor.Options.EnableEmailHyperlinks = false;
		//Editor.Options.EnableRectangularSelection = true;
	}
    
	private void Command_New_OnExecuted(object sender, RoutedEventArgs e)
	{
		viewmodel.NewDocument(this);
	}

	private void Command_Open_OnExecuted(object sender, RoutedEventArgs e)
	{
		viewmodel.LoadDocument();
	}

	private void Command_Save_OnExecuted(object sender, RoutedEventArgs e)
	{
		viewmodel.Document.SaveDocument(this);
	}

	private void Command_Export_OnExecuted(object sender, RoutedEventArgs e)
	{
		viewmodel.ExportDocument();
	}

	private void Command_SaveAs_OnExecuted(object sender, RoutedEventArgs e)
	{
		viewmodel.Document.SaveDocumentAs(this);
	}

	private void Command_Exit_OnExecuted(object sender, RoutedEventArgs e)
	{
		if (viewmodel.Document.IsChanged)
		{
			//TODO
			//if (MessageBox.Show("You have un saved changes.Would you like to save your document?", "Save Your Changes?", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
			//{
			//	if (!viewmodel.Document.SaveDocument(this)) return;
			//}
		}

		Close();
	}

	private void MainWindow_OnClosing(object sender, WindowClosingEventArgs e)
	{
		if (viewmodel.Document.IsChanged)
		{
			//TODO
			//var mbresult = MessageBox.Show("You have un saved changes.Would you like to save your document?", "Save Your Changes?", MessageBoxButton.YesNoCancel);
			//if (mbresult == MessageBoxResult.Yes)
			//{
			//	if (!viewmodel.Document.SaveDocument(this))
			//	{
			//		e.Cancel = true;
			//	}
			//}
			//else if (mbresult == MessageBoxResult.Cancel)
			//{
			//	e.Cancel = true;
			//}
		}
	}

	private void Command_Reload_OnExecuted(object sender, RoutedEventArgs e)
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
		viewmodel.Theme = PPTheme.Normal;
	}

	private void MenuItem_View_Invisible_OnClick(object sender, RoutedEventArgs e)
	{
		viewmodel.Theme = PPTheme.Invisible;
	}

	private void MenuItem_View_LowContrastDark_OnClick(object sender, RoutedEventArgs e)
	{
		viewmodel.Theme = PPTheme.LowContrastDark;
	}

	private void MenuItem_View_LowContrastLight_OnClick(object sender, RoutedEventArgs e)
	{
		viewmodel.Theme = PPTheme.LowContrastLight;
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

	private void Command_Help_OnExecuted(object sender, RoutedEventArgs e)
	{
		//TODO new AboutWindow {Owner = this}.ShowDialog();
	}

	//TODO
	//private void UIElement_OnMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
	//{
	//	viewmodel.ChangeAlgorithm(this);
	//}
}