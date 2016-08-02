using System.Reflection;

namespace Passpad
{
	/// <summary>
	/// Interaction logic for App.xaml
	/// </summary>
	public partial class App
	{
		public static readonly string VERSION = Assembly.GetEntryAssembly().GetName().Version.ToString();
	}
}


//TODO Toolbar
//TODO Password strength meter
//TODO Generate Password
