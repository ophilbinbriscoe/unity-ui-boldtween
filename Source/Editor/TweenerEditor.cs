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

			EditorGUILayout.LabelField( "Triggers", EditorStyles.boldLabel );

			EditorGUILayout.BeginVertical( EditorStyles.helpBox );

			EditorGUI.indentLevel++;
			EditorGUILayout.PropertyField( events );
			EditorGUI.indentLevel--;

			EditorGUILayout.PropertyField( initialize );

			if ( initialize.enumValueIndex > 0 )
			{
				EditorGUI.indentLevel++;
				EditorGUILayout.PropertyField( initialPosition );
				EditorGUI.indentLevel--;
			}
				
			EditorGUILayout.EndVertical();

			EditorGUILayout.LabelField( "Settings", EditorStyles.boldLabel );

			EditorGUILayout.BeginVertical( EditorStyles.helpBox );

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

			EditorGUI.indentLevel++;
			DrawList( siblings );
			EditorGUI.indentLevel--;

			EditorGUILayout.EndVertical();

			EditorGUILayout.LabelField( "Outputs", EditorStyles.boldLabel );

			EditorGUILayout.BeginVertical( EditorStyles.helpBox );

			EditorGUI.indentLevel++;

			DrawList( interpolators );

			onTweenStart.isExpanded = EditorGUILayout.Foldout( onTweenStart.isExpanded, new GUIContent( "Callbacks" ) );

			EditorGUI.indentLevel--;

			if ( onTweenStart.isExpanded )
			{
				EditorGUI.indentLevel++;
				EditorGUILayout.PropertyField( onTweenStart );
				EditorGUILayout.PropertyField( onTweenEnd );
				EditorGUILayout.PropertyField( onTweenUpdate );
				EditorGUI.indentLevel--;
			}

			EditorGUILayout.EndVertical();

			serializedObject.ApplyModifiedProperties();

			if ( Application.isPlaying )
			{
				var tweener = target as Tweener;

				EditorGUILayout.BeginHorizontal();

				EditorGUILayout.Separator();

				var rect = GUILayoutUtility.GetLastRect().Translate( 0, 8.0f );

				var content = new GUIContent( string.Format( "{0:0.00}", tweener.Position ), "Position" );

				EditorGUILayout.LabelField( content, EditorStyles.miniLabel, GUILayout.Width( 30.0f ) );

				EditorGUILayout.EndHorizontal();

				var style = EditorGUIUtility.GetBuiltinSkin( EditorSkin.Scene ).FindStyle( "ProgressBarBack" );

				GUI.Box( rect, GUIContent.none, style );

				style = EditorGUIUtility.GetBuiltinSkin( EditorSkin.Scene ).FindStyle( "ProgressBarBar" );

				GUI.Box( rect.SetWidth( rect.width * tweener.Position ), GUIContent.none, style );

				if ( GUILayout.Button( "Tween" ) )
				{
					tweener.Tween();
				}
			}
		}
	}
#endif
}