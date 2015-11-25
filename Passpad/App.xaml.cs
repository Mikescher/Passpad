using System.Reflection;
using System.Windows;

namespace Passpad
{
	/// <summary>
	/// Interaction logic for App.xaml
	/// </summary>
	public partial class App : Application
	{
		public static readonly string VERSION = Assembly.GetEntryAssembly().GetName().Version.ToString();
	}
}
