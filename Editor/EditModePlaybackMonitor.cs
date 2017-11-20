using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEditor;

public class EditModePlaybackMonitor : EditorWindow
{
	[MenuItem( "Tools/BoldTween/Show EditModePlayback Monitor" )]
	public static void ShowMonitor ()
	{
		var window = GetWindow<EditModePlaybackMonitor>();

		window.titleContent = new GUIContent( "EditModePlayback Monitor" );

		window.autoRepaintOnSceneChange = true;

		window.Show();
	}

	private void OnEnable ()
	{
		EditorApplication.update += Repaint;
	}

	private void OnGUI ()
	{
		EditorGUILayout.LabelField( "Delta Time: ", string.Format( "{0:0.000}", EditModePlayback.deltaTime ) );

		EditorGUILayout.LabelField( "Did Repaint? ", EditModePlayback.DidRepaint.ToString() );

		EditorGUILayout.LabelField( "Instances: ", EditModePlayback.ReceiverCount.ToString() );

		foreach ( var receiver in EditModePlayback.Receivers )
		{
			GUILayout.BeginHorizontal();
			GUILayout.Space( 10.0f );

			var instance = receiver as Object;

			if ( GUILayout.Button( instance.ToString(), EditorStyles.label ) )
			{
				EditorGUIUtility.PingObject( instance );
			}

			GUILayout.EndHorizontal();
		}
	}

	private void OnDisable ()
	{
		EditorApplication.update -= Repaint;
	}
}
