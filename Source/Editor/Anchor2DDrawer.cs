using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEditor;
using UnityEngine;

namespace ToBoldlyPlay.Tweening
{
	[CustomPropertyDrawer( typeof( Anchor2D ) )]
	public class Anchor2DDrawer : PropertyDrawer
	{
		private static float height = EditorGUIUtility.singleLineHeight;

		public override float GetPropertyHeight ( SerializedProperty property, GUIContent label )
		{
			return property.isExpanded ? height * 3.0f : height;
		}

		public override void OnGUI ( Rect position, SerializedProperty property, GUIContent label )
		{
			position = position.SetHeight( height );

			property.isExpanded = EditorGUI.Foldout( position, property.isExpanded, label );

			if ( property.isExpanded )
			{
				var min = property.FindPropertyRelative( "min" );
				var max = property.FindPropertyRelative( "max" );

				var width = Mathf.Min( position.width / 5.0f, 40.0f );

				var rect = position.StepDown().SetWidth( width ).StepRight( width * -0.66f );

				EditorGUI.LabelField( rect, "Min" );

				rect = rect.StepRight();

				EditorGUI.LabelField( rect.SetWidth( width * 0.5f ), "X" );

				rect = rect.StepRight( width * -0.66f );

				float x = Mathf.Clamp01( EditorGUI.FloatField( rect, GUIContent.none, min.vector2Value.x ) );

				rect = rect.StepRight();

				EditorGUI.LabelField( rect.SetWidth( width * 0.5f ), "Y" );

				rect = rect.StepRight( width * -0.66f );

				float y = Mathf.Clamp01( EditorGUI.FloatField( rect, GUIContent.none, min.vector2Value.y ) );

				min.vector2Value = new Vector2( x, y );

				rect = position.StepDown().StepDown().SetWidth( width ).StepRight( width * -0.66f );

				EditorGUI.LabelField( rect, "Max" );

				rect = rect.StepRight();

				EditorGUI.LabelField( rect.SetWidth( width * 0.5f ), "X" );

				rect = rect.StepRight( width * -0.66f );

				x = Mathf.Clamp01( EditorGUI.FloatField( rect, GUIContent.none, max.vector2Value.x ) );

				rect = rect.StepRight();

				EditorGUI.LabelField( rect.SetWidth( width * 0.5f ), "Y" );

				rect = rect.StepRight( width * -0.66f );

				y = Mathf.Clamp01( EditorGUI.FloatField( rect, GUIContent.none, max.vector2Value.y ) );

				max.vector2Value = new Vector2( x, y );

				position = position.StepDown();
			}
		}
	}
}
