using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Passpad.Dialogs
{
	/// <summary>
	/// Interaction logic for ChangePasswordDialog.xaml
	/// </summary>
	public partial class ChangePasswordDialog : Window
	{
		public string Password = null;

		public ChangePasswordDialog()
		{
			InitializeComponent();
		}

		public bool ShowDialog(Window owner, string value)
		{
			Owner = owner;

			PasswordBox.Password = value;
			PasswordBox.Focus();
			Keyboard.Focus(PasswordBox);
			SetSelection(PasswordBox, PasswordBox.Password.Length, 0);

			if (ShowDialog() ?? false)
			{
				Password = PasswordBox.Password;
				return true;
			}
			else
			{
				Password = null;
				return false;
			}
		}

		private void Button_Ok_Click(object sender, RoutedEventArgs e)
		{
			DialogResult = true;
			Close();
		}

		private void Button_Cancel_Click(object sender, RoutedEventArgs e)
		{
			DialogResult = false;
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

		private void ChangePasswordDialog_OnPreviewKeyDown(object sender, KeyEventArgs e)
		{
			if (e.Key == Key.Enter && BtnOK.IsEnabled)
			{
				Button_Ok_Click(sender, e);
				e.Handled = true;
			}

			if (e.Key == Key.Escape)
			{
				Button_Cancel_Click(sender, e);
				e.Handled = true;
			}
		}
	}
}
