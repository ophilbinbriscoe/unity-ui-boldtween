using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEditor;
using UnityEditorInternal;

namespace BoldTween.Sequences
{
	[CustomEditor( typeof( Sequence ) )]
	public class SequenceEditor : Editor
	{
		private static List<Type> elementType;
		private static GUIContent[] elementsContent;

		public MonoScript upgradeEffectSource;

		static SequenceEditor ()
		{
			var baseType = typeof( SequenceElement );

			elementType = new List<Type>();

			foreach ( var type in baseType.Assembly.GetTypes() )
			{
				// we only want to deal with fully-implemented elements
				if ( type.IsAbstract )
				{
					continue;
				}

				// check if the type is derrived from SequenceElement
				if ( baseType.IsAssignableFrom( type ) )
				{
					elementType.Add( type );
				}
			}

			elementsContent = new GUIContent[elementType.Count];

			for ( int i = 0; i < elementType.Count; i++ )
			{
				elementsContent[i] = new GUIContent( NiceTypeName( elementType[i] ) );
			}
		}

		private static string NiceTypeName ( Type type )
		{
			return /*ObjectNames.NicifyVariableName( */type.Name/*.Split( new char[] { '.' }, StringSplitOptions.RemoveEmptyEntries ).Last() )*/;
		}

		SerializedProperty elementsProperty;
		ReorderableList elementsList;

		private void OnEnable ()
		{
			elementsProperty = serializedObject.FindProperty( "elements" );

			InitializeElementsList();
		}

		private void InitializeElementsList ()
		{
			elementsList = new ReorderableList( serializedObject, elementsProperty );

			elementsList.onAddDropdownCallback = OnAddDropdown;
			elementsList.onRemoveCallback = OnRemove;
			elementsList.drawHeaderCallback = DrawHeader;
			elementsList.elementHeightCallback = ElementHeight;
			elementsList.drawElementCallback = DrawElement;
		}

		#region Effects List Callbacks
		private void DrawHeader ( Rect rect )
		{
			EditorGUI.LabelField( rect, "Elements" );
		}

		private float ElementHeight ( int index )
		{
			float height = EditorGUIUtility.singleLineHeight + 12f;

			var effectProperty = elementsProperty.GetArrayElementAtIndex( index );

			if ( effectProperty.isExpanded )
			{
				height += EditorGUI.GetPropertyHeight( effectProperty );
			}

			return height;
		}

		private void DrawElement ( Rect rect, int index, bool isActive, bool isFocuses )
		{
			rect = rect.PadBottom( 6f ).PadTop( 2f );

			GUI.Box( rect, GUIContent.none, (GUIStyle) "TE NodeBox" );

			var effectProperty = elementsProperty.GetArrayElementAtIndex( index );

			var effect = effectProperty.objectReferenceValue;

			var headerText = effect == null ? "(missing)" : NiceTypeName( effect.GetType() );
			var headerIcon = effect == null ? EditorGUIUtility.Load( "icons/d_console.warnicon.sml.png" ) as Texture : upgradeEffectSource == null ? null : AssetDatabase.GetCachedIcon( AssetDatabase.GetAssetPath( upgradeEffectSource ) );

			var headerRect = rect.SetHeight( EditorGUIUtility.singleLineHeight ).PadLeft( 12f ).PadHorizontal( 2f ).DisplaceY( 2f );
			var headerStyle = new GUIStyle( EditorStyles.foldout );

			Color color = headerStyle.normal.textColor * 1.2f;

			headerStyle.fontStyle = FontStyle.Bold;

			headerStyle.normal.textColor = color;
			headerStyle.onNormal.textColor = color;
			headerStyle.hover.textColor = color;
			headerStyle.onHover.textColor = color;
			headerStyle.active.textColor = color;
			headerStyle.onActive.textColor = color;
			headerStyle.focused.textColor = color;
			headerStyle.onFocused.textColor = color;

			if ( headerRect.WasLeftClicked( true ) )
			{
				Selection.activeObject = effect;
			}

			if ( effectProperty.isExpanded = EditorGUI.Foldout( headerRect, effectProperty.isExpanded, new GUIContent( string.Format( " {0}", headerText ), headerIcon ), headerStyle ) )
			{
				var effectPropertiesRect = rect.PadTop( EditorGUIUtility.singleLineHeight + 2f ).PadHorizontal( 4f ).PadRight( 1f );

				GUI.Box( effectPropertiesRect.PadHorizontal( -2f ).PadBottom( 1f ), GUIContent.none, (GUIStyle) "CN Box" );

				EditorGUI.PropertyField( effectPropertiesRect.PadTop( 2f ), effectProperty );
			}
		}

		private void OnAddDropdown ( Rect buttonRect, ReorderableList effectsList )
		{
			EditorUtility.DisplayPopupMenu( buttonRect, "Upgrade Effects/", null );
			EditorUtility.DisplayCustomMenu( buttonRect, elementsContent, -1, OnSelectUpgradeEffectToAdd, null, false );
		}

		private void OnSelectUpgradeEffectToAdd ( object userData, string[] options, int selected )
		{
			if ( selected > -1 )
			{
				serializedObject.Update();

				int index = elementsProperty.arraySize;

				elementsProperty.InsertArrayElementAtIndex( index );

				var elementProperty = elementsProperty.GetArrayElementAtIndex( index );

				var element = CreateInstance( elementType[selected] );

				element.name = elementsContent[selected].text;

				element.hideFlags = HideFlags.HideInHierarchy;

				elementProperty.objectReferenceValue = element;

				serializedObject.ApplyModifiedProperties();

				Undo.RegisterCreatedObjectUndo( element, "Create Sequence Element" );
			}
		}

		private void OnRemove ( ReorderableList effectsList )
		{
			int index = effectsList.index;

			var effectProperty = elementsProperty.GetArrayElementAtIndex( index );

			var effect = effectProperty.objectReferenceValue;

			if ( effect != null )
			{
				Undo.DestroyObjectImmediate( effect );
			}

			effectProperty.objectReferenceValue = null;

			elementsProperty.DeleteArrayElementAtIndex( index );
		}
		#endregion

		public override void OnInspectorGUI ()
		{
			serializedObject.Update();

			EditorGUILayout.Space();

			DrawPropertiesExcluding( serializedObject, "m_Script", "elements" );

			elementsList.DoLayoutList();

			serializedObject.ApplyModifiedProperties();
		}
	}
}
