using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEditor;

namespace BoldTween
{
	[CustomEditor( typeof( InOut ) )]
	public class InOutEditor : Editor
	{
		public override void OnInspectorGUI ()
		{
			base.OnInspectorGUI();

			var tweener = target as InOut;

			GUILayout.BeginHorizontal();

			if ( GUILayout.Button( "In", EditorStyles.miniButtonLeft ) )
			{
				if ( Application.isPlaying )
				{
					tweener.In();
				}
				else
				{
					serializedObject.FindProperty( "previewDirection" ).enumValueIndex = 1;

					serializedObject.ApplyModifiedProperties();

					EditModePlayback.Register( tweener );
				}
			}

			if ( GUILayout.Button( "Out", EditorStyles.miniButtonRight ) )
			{
				if ( Application.isPlaying )
				{
					tweener.Out();
				}
				else
				{
					serializedObject.FindProperty( "previewDirection" ).enumValueIndex = 2;

					serializedObject.ApplyModifiedProperties();

					EditModePlayback.Register( tweener );
				}
			}

			if ( GUILayout.Button( "Stop", EditorStyles.miniButton ) )
			{
				if ( Application.isPlaying )
				{
					tweener.Stop();
				}
				else
				{
					serializedObject.FindProperty( "previewDirection" ).enumValueIndex = 0;

					serializedObject.ApplyModifiedProperties();
				}
			}

			GUILayout.EndHorizontal();
		}
	}
}
