using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;

namespace BoldTween
{
	public class TwistInterpolator : RectTransformInterpolator<float>
	{
		protected override float Interpolate ( float a, float b, float t )
		{
			return a + (b - a) * t;
		}

		protected override void OnInterpolate ( RectTransform target, float value )
		{
			tracker.Clear();
			tracker.Add( this, target, DrivenTransformProperties.Rotation );

			target.localRotation = Quaternion.Euler( 0.0f, 0.0f, value );
		}
	}
}
