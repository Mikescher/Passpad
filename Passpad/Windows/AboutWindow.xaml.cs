using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Input;

namespace Passpad
{
	/// <summary>
	/// Interaction logic for AboutWindow.xaml
	/// </summary>
	public partial class AboutWindow : Window
	{
		public AboutWindow()
		{
			InitializeComponent();

			TitleBox.Text = "Passpad v" + App.VERSION;
		}

		private void UIElement_mikescher_OnMouseDown(object sender, MouseButtonEventArgs e)
		{
			Process.Start("http://www.mikescher.de");
		}

		private void UIElement_mono_OnMouseDown(object sender, MouseButtonEventArgs e)
		{
			Process.Start("http://www.customicondesign.com/free-icons/mono-icon-set/mono-general-4/");
		}

		private void UIElement_avalon_OnMouseDown(object sender, MouseButtonEventArgs e)
		{
			Process.Start("http://avalonedit.net/");
		}
	}
}
