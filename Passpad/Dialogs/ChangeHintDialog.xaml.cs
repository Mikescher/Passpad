using System.Windows;
using System.Windows.Input;

namespace Passpad.Dialogs
{
	/// <summary>
	/// Interaction logic for ChangeHintDialog.xaml
	/// </summary>
	public partial class ChangeHintDialog : Window
	{
		public string Hint = null;

		public ChangeHintDialog()
		{
			InitializeComponent();
		}

		public bool ShowDialog(Window owner, string value)
		{
			Owner = owner;

			HintBox.Text = value;

			HintBox.Focus();
			Keyboard.Focus(HintBox);

			if (ShowDialog() ?? false)
			{
				Hint = HintBox.Text;
				return true;
			}
			else
			{
				Hint = null;
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

		private void ChangeHintDialog_OnPreviewKeyDown(object sender, KeyEventArgs e)
		{
			if (e.Key == Key.Escape)
			{
				Button_Cancel_Click(sender, e);
				e.Handled = true;
			}
		}
	}
}
