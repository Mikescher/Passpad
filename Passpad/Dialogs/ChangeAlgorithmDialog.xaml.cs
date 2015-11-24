using System;
using System.Windows;
using Passpad.Encryption;

namespace Passpad.Dialogs
{
	/// <summary>
	/// Interaction logic for ChangeAlgorithmDialog.xaml
	/// </summary>
	public partial class ChangeAlgorithmDialog : Window
	{
		public EncryptionAlgorithm Algorithm = EncryptionAlgorithm.Plain;

		public ChangeAlgorithmDialog()
		{
			InitializeComponent();
		}

		public bool ShowDialog(Window owner, EncryptionAlgorithm value)
		{
			Owner = owner;

			AlgorithmBox.Items.Clear();
			foreach (var enumvalue in Enum.GetValues(typeof (EncryptionAlgorithm)))
			{
				AlgorithmBox.Items.Add(enumvalue);
			}

			AlgorithmBox.SelectedItem = value;

			if (ShowDialog() ?? false)
			{
				Algorithm = (EncryptionAlgorithm) AlgorithmBox.SelectedItem;
				return true;
			}
			else
			{
				Algorithm = EncryptionAlgorithm.Plain;
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
	}
}
