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
		private int EnumValueIndex ( Direction direction )
		{
			return Array.IndexOf( Enum.GetValues( typeof( Direction ) ) as Direction[], direction );
		}

		public override void OnInspectorGUI ()
		{
			serializedObject.Update();

			EditorGUILayout.Space();

			DrawPropertiesExcluding( serializedObject, "m_Script", "onTween" );

			var tweener = target as InOut;

			EditorGUILayout.Space();

			GUILayout.BeginHorizontal();

			if ( GUILayout.Button( "In", EditorStyles.miniButtonLeft ) )
			{
				if ( Application.isPlaying )
				{
					tweener.In();
				}
				else
				{
					serializedObject.FindProperty( "previewDirection" ).enumValueIndex = EnumValueIndex( Direction.In );

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
					serializedObject.FindProperty( "previewDirection" ).enumValueIndex = EnumValueIndex( Direction.Out );

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
					serializedObject.FindProperty( "previewDirection" ).enumValueIndex = EnumValueIndex( Direction.None );
				}
			}

			using ( new EditorGUI.DisabledScope( true ) )
			{
				EditorGUILayout.PropertyField( serializedObject.FindProperty( "previewDirection" ), GUIContent.none );
			}

			GUILayout.EndHorizontal();

			EditorGUILayout.Space();

			EditorGUILayout.PropertyField( serializedObject.FindProperty( "onTween" ) );

			serializedObject.ApplyModifiedProperties();
		}
	}
}
