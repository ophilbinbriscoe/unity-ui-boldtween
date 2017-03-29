using UnityEditor;

namespace ToBoldlyPlay
{
	[InitializeOnLoad]
	public static class Definitions_Tween
	{
		static Definitions_Tween ()
		{
			var targetGroup = BuildPipeline.GetBuildTargetGroup( EditorUserBuildSettings.activeBuildTarget );

			var defines = PlayerSettings.GetScriptingDefineSymbolsForGroup( targetGroup );

			if ( !defines.Contains( "BOLD_TWEEN" ) )
			{
				defines += ";BOLD_TWEEN";
			}

			PlayerSettings.SetScriptingDefineSymbolsForGroup( targetGroup, defines );
		}
	}
}