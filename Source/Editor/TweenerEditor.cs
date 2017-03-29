using System;
using System.Globalization;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;
using UnityEditor;

using UnityEditorInternal;

namespace ToBoldlyPlay.Tweening
{
#if BOLD_EDITOR
	[CustomEditor( typeof( Tweener ) )]
	public class TweenerEditor : BoldEditor
	{
		SerializedProperty
			events,
			interpolators,
			siblings,
			onTweenStart, onTweenEnd, onTweenUpdate,
			type,
			preset, multiplier,
			curve, duration,
			modifiers,
			initialize, initialPosition,
			timeType;

		private void OnEnable ()
		{
			events = serializedObject.FindProperty( "events" );
			interpolators = serializedObject.FindProperty( "interpolators" );
			siblings = serializedObject.FindProperty( "siblings" );
			onTweenStart = serializedObject.FindProperty( "onTweenStart" );
			onTweenEnd = serializedObject.FindProperty( "onTweenEnd" );
			onTweenUpdate = serializedObject.FindProperty( "onTweenUpdate" );
			type = serializedObject.FindProperty( "type" );
			preset = serializedObject.FindProperty( "preset" );
			curve = serializedObject.FindProperty( "curve" );
			duration = serializedObject.FindProperty( "duration" );
			modifiers = serializedObject.FindProperty( "modifiers" );
			multiplier = serializedObject.FindProperty( "multiplier" );
			initialize = serializedObject.FindProperty( "initialize" );
			initialPosition = serializedObject.FindProperty( "initialPosition" );
			timeType = serializedObject.FindProperty( "timeType" );
		}

		public override void OnInspectorGUI ()
		{
			serializedObject.Update();

			EditorGUILayout.Space();

			EditorGUILayout.PropertyField( events );

			EditorGUILayout.PropertyField( initialize );

			if ( initialize.enumValueIndex > 0 )
			{
				EditorGUI.indentLevel++;

				EditorGUILayout.PropertyField( initialPosition );

				EditorGUI.indentLevel--;
			}

			EditorGUILayout.PropertyField( type );

			EditorGUI.indentLevel++;

			if ( type.boolValue )
			{
				EditorGUILayout.PropertyField( preset );
				EditorGUILayout.PropertyField( multiplier );
			}
			else
			{
				EditorGUILayout.PropertyField( curve );
				EditorGUILayout.PropertyField( duration );
			}

			EditorGUI.indentLevel--;

			EditorGUILayout.PropertyField( modifiers );
			EditorGUILayout.PropertyField( timeType );

			EditorGUILayout.Space();

			DrawList( interpolators );

			DrawList( siblings );

			onTweenStart.isExpanded = EditorGUILayout.Foldout( onTweenStart.isExpanded, new GUIContent( "Callbacks" ) );

			if ( onTweenStart.isExpanded )
			{
				EditorGUI.indentLevel++;

				EditorGUILayout.PropertyField( onTweenStart );
				EditorGUILayout.PropertyField( onTweenEnd );
				EditorGUILayout.PropertyField( onTweenUpdate );

				EditorGUI.indentLevel--;
			}

			serializedObject.ApplyModifiedProperties();

			if ( Application.isPlaying )
			{
				if ( GUILayout.Button( "Tween" ) )
				{
					(target as Tweener).Tween();
				}
			}
		}
	}
#endif
}