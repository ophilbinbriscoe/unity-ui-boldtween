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
	private static HashSet<IEditModePlayback> receivers;

	public static int ReceiverCount
	{
		get
		{
			return receivers.Count;
		}
	}

	public static IEnumerable<IEditModePlayback> Receivers
	{
		get
		{
			foreach ( var receiver in receivers )
			{
				yield return receiver;
			}
		}
	}

	static EditModePlayback ()
	{
		receivers = new HashSet<IEditModePlayback>();

		EditorApplication.update += Update;

		time = EditorApplication.timeSinceStartup;
	}

	private static double time;

	public static float deltaTime { get; private set; }

	private static bool repaint;

	public static bool DidRepaint
	{
		get
		{
			return repaint;
		}
	}

	private static void Update ()
	{
		deltaTime = (float) (EditorApplication.timeSinceStartup - time);

		repaint = false;

		foreach ( var receiver in register )
		{
			receivers.Add( receiver );
		}

		register.Clear();

		if ( !Application.isPlaying && receivers.Count > 0 )
		{
			foreach ( var receiver in receivers )
			{
				if ( receiver.EditModeUpdate() )
				{
					unregister.Add( receiver );
				}

				repaint |= receiver.RequiresEditModeRepaint;
			}

			if ( repaint )
			{
				UnityEditorInternal.InternalEditorUtility.RepaintAllViews();
			}
		}

		foreach ( var receiver in unregister )
		{
			receivers.Remove( receiver );
		}

		unregister.Clear();

		time = EditorApplication.timeSinceStartup;
	}

	private static List<IEditModePlayback> register = new List<IEditModePlayback>();

	public static void Register ( IEditModePlayback receiver )
	{
		register.Add( receiver );
	}

	private static List<IEditModePlayback> unregister = new List<IEditModePlayback>();

	public static void Unregister ( IEditModePlayback receiver )
	{
		unregister.Add( receiver );
	}
}
#endif