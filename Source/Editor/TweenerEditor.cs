using System;
using System.Globalization;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;
using UnityEditor;

namespace ToBoldlyPlay.Tweening
{
	[CustomEditor( typeof( Tweener ) )]
	public class TweenerEditor
#if BOLD_EDITOR
		: ExtendedEditor
#else
		: Editor
#endif
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
				EditorGUILayout.PropertyField( modifiers );
			}

			EditorGUI.indentLevel--;

			EditorGUILayout.PropertyField( timeType );

			EditorGUI.indentLevel++;
#if BOLD_EDITOR
			DrawList( siblings );
#else
			EditorGUILayout.PropertyField( siblings );
#endif
			EditorGUI.indentLevel--;

			EditorGUILayout.EndVertical();

			EditorGUILayout.LabelField( "Side-Effects", EditorStyles.boldLabel );

			EditorGUILayout.BeginVertical( EditorStyles.helpBox );

			EditorGUI.indentLevel++;

#if BOLD_EDITOR
			DrawList( interpolators );
#else
			EditorGUILayout.PropertyField( interpolators );
#endif

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

				EditorGUILayout.BeginHorizontal();

				using ( var disabled = new EditorGUI.DisabledScope( !tweener.IsTweening || tweener.IsPaused ) )
				{
					if ( GUILayout.Button( "Pause", EditorStyles.miniButtonLeft ) )
					{
						tweener.Pause();
					}
				}

				using ( var disabled = new EditorGUI.DisabledScope( !tweener.IsTweening || !tweener.IsPaused ) )
				{
					if ( GUILayout.Button( "Resume", EditorStyles.miniButtonMid ) )
					{
						tweener.Resume();
					}
				}

				using ( var disabled = new EditorGUI.DisabledScope( !tweener.IsTweening ) )
				{
					if ( GUILayout.Button( "Stop", EditorStyles.miniButtonRight ) )
					{
						tweener.Stop();
					}
				}

				EditorGUILayout.EndHorizontal();
			}
		}

		public override bool RequiresConstantRepaint ()
		{
			return (target as Tweener).IsTweening;
		}
	}
}