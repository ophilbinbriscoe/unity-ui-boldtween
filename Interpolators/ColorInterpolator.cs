using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

namespace BoldTween
{
	[Serializable]
	public class ColorInterpolator : Interpolator<Color>
	{
		[Serializable]
		private class InterpolatorEvent : UnityEvent<Color> { }

		[SerializeField]
		private InterpolatorEvent onInterpolate;

#if UNITY_EDITOR
		private void Reset ()
		{
			a = Color.clear;
			b = Color.white;
		}
#endif

		protected override Color Interpolate ( Color a, Color b, float t )
		{
			return a + (b - a) * t;
		}

		protected override void OnInterpolate ( Color value )
		{
			onInterpolate.Invoke( value );
		}
	}
}
