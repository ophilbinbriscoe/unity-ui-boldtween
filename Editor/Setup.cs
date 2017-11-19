using UnityEditor;
using PluginUtilities;

namespace BoldTween
{
	[InitializeOnLoad]
	public class Setup
	{
		private static readonly Installer installer;

		static Setup ()
		{
			installer = new Installer( Constants.PLUGIN_NAME, Constants.SCRIPTING_DEFINE, new System.Version( Constants.VERSION ) );

			installer.Auto();
		}

#if BOLD_TWEEN
		[MenuItem( "Tools/" + Constants.PLUGIN_NAME + "/Reinstall" )]
		private static void Reinstall ()
		{
			installer.Uninstall( showDialogs: false );
			installer.Install();
		}

		[MenuItem( "Tools/" + Constants.PLUGIN_NAME + "/Uninstall" )]
		private static void Uninstall ()
		{
			installer.Uninstall();
		}
#else
		[MenuItem( "Tools/" + Constants.PLUGIN_NAME + "/Install" )]
		private static void Install ()
		{
			installer.Install();
		}
#endif
	}
}
