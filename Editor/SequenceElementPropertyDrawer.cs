using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEditor;

namespace BoldTween.Sequences
{
	[CustomPropertyDrawer( typeof( SequenceElement ) )]
	public class UpgradeEffectDrawer : PropertyDrawer
	{
		private static Dictionary<UnityEngine.Object, SerializedObject> serializedElementObjects = new Dictionary<UnityEngine.Object, SerializedObject>();

		private SerializedObject GetSerializedElementObject ( SerializedProperty elementProperty )
		{
			var effect = elementProperty.objectReferenceValue;

			SerializedObject serializedObject;

			if ( !serializedElementObjects.TryGetValue( effect, out serializedObject ) )
			{
				serializedElementObjects.Add( effect, serializedObject = new SerializedObject( effect ) );
			}

			return serializedObject;
		}

		public override float GetPropertyHeight ( SerializedProperty property, GUIContent label )
		{
			if ( property.objectReferenceValue != null )
			{
				var serializedObject = GetSerializedElementObject( property );

				serializedObject.Update();

				var iterator = serializedObject.GetIterator();

				// skip the "Script" field
				iterator.NextVisible( true );

				float height = 0f;

				if ( iterator.NextVisible( true ) )
				{
					do
					{
						height += EditorGUI.GetPropertyHeight( iterator, GUIContent.none, true );
						height += 2f;
					}
					while ( iterator.NextVisible( false ) );
				}

				return height;
			}

			return EditorGUIUtility.singleLineHeight;
		}

		public override void OnGUI ( Rect position, SerializedProperty property, GUIContent label )
		{
			if ( property.objectReferenceValue != null )
			{
				var serializedObject = GetSerializedElementObject( property );

				serializedObject.Update();

				var iterator = serializedObject.GetIterator();

				// skip the "Script" field
				iterator.NextVisible( true );

				if ( iterator.NextVisible( true ) )
				{
					position = position.PadLeft( 12f );

					do
					{
						label = new GUIContent( iterator.displayName, iterator.tooltip );

						position.height = EditorGUI.GetPropertyHeight( iterator, label, true );

						EditorGUI.PropertyField( position, iterator, label, true );

						position = position.StepDown( 2f );
					}
					while ( iterator.NextVisible( false ) );

					serializedObject.ApplyModifiedProperties();
				}
			}
			else
			{
				EditorGUI.LabelField( position, "(missing)", EditorStyles.centeredGreyMiniLabel );
			}
		}
	}
}