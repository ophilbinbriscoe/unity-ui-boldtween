#if UNITY_EDITOR
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public interface IEditModePlayback
{
	bool EditModeUpdate ();
	bool RequiresEditModeRepaint { get; }
}

[InitializeOnLoad]
public static class EditModePlayback
{
	[MenuItem( "Tools/BoldTween/Print Registered IEditModePlayback Count" )]
	public static void PrintRegisteredCount ()
	{
		Debug.LogFormat( "Registered IEditModePlayback Count: {0}", receivers.Count );
	}

	private static HashSet<IEditModePlayback> receivers;

	static EditModePlayback ()
	{
		receivers = new HashSet<IEditModePlayback>();

		EditorApplication.update += Update;

		time = EditorApplication.timeSinceStartup;
	}

	private static double time;

	public static float deltaTime { get; private set; }

	private static void Update ()
	{
		deltaTime = (float) (EditorApplication.timeSinceStartup - time);

		if ( !Application.isPlaying && receivers.Count > 0 )
		{
			var done = new List<IEditModePlayback>();

			var repaint = false;

			foreach ( var receiver in receivers )
			{
				if ( receiver.EditModeUpdate() )
				{
					done.Add( receiver );

					repaint |= receiver.RequiresEditModeRepaint;
				}
			}

			foreach ( var receiver in done )
			{
				receivers.Remove( receiver );
			}

			if ( repaint )
			{
				UnityEditorInternal.InternalEditorUtility.RepaintAllViews();
			}
		}

		time = EditorApplication.timeSinceStartup;
	}

	public static void Register ( IEditModePlayback receiver )
	{
		receivers.Add( receiver );
	}

	public static void Unregister ( IEditModePlayback receiver )
	{
		receivers.Remove( receiver );
	}
}
#endif