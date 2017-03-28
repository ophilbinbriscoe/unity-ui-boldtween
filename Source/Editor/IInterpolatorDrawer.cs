using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEditor;
using UnityEngine;

namespace ToBoldlyPlay.Tweening
{
	[CustomPropertyDrawer( typeof( IInterpolator ) )]
	public class IInterpolatorDrawer : PropertyDrawer
	{
		public override void OnGUI ( Rect position, SerializedProperty property, GUIContent label )
		{
			EditorGUI.ObjectField( position, property, typeof( IInterpolator ), label );
		}
	}
}
