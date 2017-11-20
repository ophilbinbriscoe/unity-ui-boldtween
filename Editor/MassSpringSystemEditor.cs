using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEditor;

namespace BoldTween
{
	[CustomEditor( typeof( MassSpringSystem ) )]
	public class MassSpringSystemEditor : Editor
	{
		SerializedProperty positionProperty;
		SerializedProperty currentPositionProperty;

		private void OnEnable ()
		{
			positionProperty = serializedObject.FindProperty( "position" );
			currentPositionProperty = serializedObject.FindProperty( "springPosition" );
		}

		public override void OnInspectorGUI ()
		{
			serializedObject.Update();

			EditorGUILayout.Space();

			EditorGUILayout.PropertyField( positionProperty, new GUIContent( "Target Position", "Goal for the mass spring system." ) );

			using ( new EditorGUI.DisabledScope( true ) )
			{
				EditorGUILayout.PropertyField( currentPositionProperty );
			}

			DrawPropertiesExcluding( serializedObject, "m_Script", "position", "springPosition" );

			serializedObject.ApplyModifiedProperties();
		}
	}
}
