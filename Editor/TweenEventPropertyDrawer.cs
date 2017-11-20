using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEditor;
using UnityEditor.UI;
using UnityEditor.Events;

namespace BoldTween
{
	[CustomPropertyDrawer( typeof( TweenEvent ) )]
	public class TweenEventPropertyDrawer : PropertyDrawer
	{
		private float GetControlHeight ()
		{
			return EditorGUIUtility.singleLineHeight;
		}

		private float GetControlsHeight ()
		{
			return 1.0f * EditorGUIUtility.singleLineHeight + 4.0f;
		}

		private SerializedProperty GetInnerEventProperty ( SerializedProperty property )
		{
			return property.FindPropertyRelative( "innerEvent" );
		}

		public override float GetPropertyHeight ( SerializedProperty property, GUIContent label )
		{
			var innerEventProperty = GetInnerEventProperty( property );

			return EditorGUI.GetPropertyHeight( innerEventProperty, label ) + GetControlsHeight();
		}

		public override void OnGUI ( Rect position, SerializedProperty property, GUIContent label )
		{
			//var interpolatorToAddProperty = property.FindPropertyRelative( "interpolatorToAdd" );

			//EditorGUI.BeginChangeCheck();
			//EditorGUI.PropertyField( new Rect( position.x, position.y, position.width, GetControlHeight() ), interpolatorToAddProperty );

			//if ( EditorGUI.EndChangeCheck() )
			//{

			//}

			var interpolatorsToAddProperty = property.FindPropertyRelative( "interpolatorsToAdd" );

			var interpolator = EditorGUI.ObjectField( new Rect( position.x, position.y, position.width, GetControlHeight() ), new GUIContent( "Add target(s)...", "Bind all of the listeners on a particular GameObject." ), null, typeof( Interpolator ), true ) as Interpolator;

			if ( interpolator != null )
			{
				interpolatorsToAddProperty.objectReferenceValue = interpolator.gameObject;
			}

			var innerEventProperty = GetInnerEventProperty( property );

			EditorGUI.PropertyField( new Rect( position.x, position.y + GetControlsHeight(), position.width, position.height - GetControlsHeight() ), innerEventProperty, label );
		}
	}
}
