using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;

namespace BoldTween.UI
{
	public class UIPositionInterpolator : RectTransformInterpolator<Vector2>
	{
		protected override void Reset ()
		{
			base.Reset();

			a = target.anchoredPosition;
			b = target.anchoredPosition;
		}

		protected override void OnInterpolate ( RectTransform target, Vector2 value )
		{
			tracker.Clear();
			tracker.Add( this, target, DrivenTransformProperties.AnchoredPositionX | DrivenTransformProperties.AnchoredPositionY );

			target.anchoredPosition = value;
		}

		protected override Vector2 Interpolate ( Vector2 a, Vector2 b, float t )
		{
			return a + (b - a) * t;
		}
	}
}