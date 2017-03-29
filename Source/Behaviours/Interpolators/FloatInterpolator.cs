using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

namespace ToBoldlyPlay.Tweening
{
	public class FloatInterpolator : Interpolator<float>
	{
		[Serializable]
		public class InterpolateEvent : UnityEvent<float> { }

		public InterpolateEvent onInterpolate;

		public override void Interpolate ( float t )
		{
			onInterpolate.Invoke( Mathf.Lerp( a, b, t ) );
		}
	}
}
