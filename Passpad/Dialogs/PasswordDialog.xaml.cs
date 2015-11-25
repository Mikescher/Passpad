using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Passpad.Dialogs
{
	public partial class PasswordDialog
	{
		private PasswordDialog()
		{
			InitializeComponent();
		}

		public static string ShowDialog(Window owner, string hint)
		{
			var window = new PasswordDialog {Owner = owner};
			
			window.PasswordBox.Focus();
			window.HintBox.Text = hint;
			Keyboard.Focus(window.PasswordBox);

			if (window.ShowDialog() ?? false)
			{
				return window.PasswordBox.Password;
			}
			else
			{
				return string.Empty;
			}
		}

		private void Button_Ok_Click(object sender, RoutedEventArgs e)
		{
			DialogResult = true;
			Close();
		}

		private void PasswordBox_OnPasswordChanged(object sender, RoutedEventArgs e)
		{
			BtnOK.IsEnabled = ! string.IsNullOrWhiteSpace(PasswordBox.Password);
		}

		private void Button_ShowPassword_Clicked(object sender, RoutedEventArgs e)
		{
			if (PasswordBox.Visibility == Visibility.Visible)
			{
				PasswordBox.Visibility = Visibility.Hidden;
				PasswordBoxPlain.Visibility = Visibility.Visible;

				PasswordBoxPlain.Text = PasswordBox.Password;
				PasswordBoxPlain.SelectionStart = PasswordBoxPlain.Text.Length;
				PasswordBoxPlain.SelectionLength = 0;

				PasswordBoxPlain.Focus();
				Keyboard.Focus(PasswordBoxPlain);
			}
			else
			{
				PasswordBox.Visibility = Visibility.Visible;
				PasswordBoxPlain.Visibility = Visibility.Hidden;

				PasswordBox.Password = PasswordBoxPlain.Text;
				SetSelection(PasswordBox, PasswordBox.Password.Length, 0);

				PasswordBox.Focus();
				Keyboard.Focus(PasswordBox);
			}
		}

		private void SetSelection(PasswordBox passwordBox, int start, int length)
		{
			passwordBox.GetType().GetMethod("Select", BindingFlags.Instance | BindingFlags.NonPublic).Invoke(passwordBox, new object[] { start, length });
		}

		private void PasswordDialog_OnPreviewKeyDown(object sender, KeyEventArgs e)
		{
			if (e.Key == Key.Enter && BtnOK.IsEnabled) Button_Ok_Click(sender, e);
		}
	}
}
