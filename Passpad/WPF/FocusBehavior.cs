using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Passpad.WPF
{
	public static class FocusBehavior
	{
		public static readonly DependencyProperty WPFFocusFirstProperty =
			DependencyProperty.RegisterAttached(
				"WPFFocusFirst",
				typeof(bool),
				typeof(Control),
				new PropertyMetadata(false, OnWPFFocusFirstPropertyChanged));

		public static bool GetWPFFocusFirst(Control control)
		{
			return (bool)control.GetValue(WPFFocusFirstProperty);
		}

		public static void SetWPFFocusFirst(Control control, bool value)
		{
			control.SetValue(WPFFocusFirstProperty, value);
		}

		static void OnWPFFocusFirstPropertyChanged(DependencyObject obj, DependencyPropertyChangedEventArgs args)
		{
			Control control = obj as Control;
			if (control == null || !(args.NewValue is bool))
			{
				return;
			}

			if ((bool)args.NewValue)
			{
				control.Loaded += (sender, e) =>
				{
					control.MoveFocus(new TraversalRequest(FocusNavigationDirection.Next));
				};
			}
		}
	}
}
