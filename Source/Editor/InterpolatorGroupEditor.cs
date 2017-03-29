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
	[CustomEditor( typeof( InterpolatorGroup ) )]
	public class InterpolatorGroupEditor : BoldEditor
	{
		SerializedProperty interpolators;

		private void OnEnable ()
		{
			interpolators = serializedObject.FindProperty( "interpolators" );
		}

		public override void OnInspectorGUI ()
		{
			serializedObject.Update();

			EditorGUILayout.Space();

			DrawList( interpolators );

			serializedObject.ApplyModifiedProperties();
		}
	}
#endif
}