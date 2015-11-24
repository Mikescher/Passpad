using System.Windows.Input;

namespace Passpad
{
	public static class CustomCommands
	{
		public static readonly RoutedUICommand Export = new RoutedUICommand
		(
				"Export",
				"Export",
				typeof(CustomCommands),
				new InputGestureCollection() { new KeyGesture(Key.E, ModifierKeys.Control) }
		);

		public static readonly RoutedUICommand Reload = new RoutedUICommand
		(
				"Reload",
				"Reload",
				typeof(CustomCommands),
				new InputGestureCollection()
		);
	}

}
