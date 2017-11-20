using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;

namespace BoldTween.UI
{
	public class UIScaleInterpolator : RectTransformInterpolator<Vector2>
	{
		protected override void Reset ()
		{
			base.Reset();

			a = target.localScale;
			b = target.localScale;
		}

		protected override void OnInterpolate ( RectTransform target, Vector2 value )
		{
			tracker.Clear();
			tracker.Add( this, target, DrivenTransformProperties.ScaleX | DrivenTransformProperties.ScaleY );

			target.localScale = new Vector3( value.x, value.y, target.localScale.z );
		}

		protected override Vector2 Interpolate ( Vector2 a, Vector2 b, float t )
		{
			return a + (b - a) * t;
		}
	}
}