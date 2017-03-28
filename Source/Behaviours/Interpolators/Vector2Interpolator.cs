using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

namespace ToBoldlyPlay.Tweening
{
	public class Vector2Interpolator : Interpolator<Vector2>
	{
		[Serializable]
		public class InterpolateEvent : UnityEvent<Vector2> { }

		[SerializeField]
		public InterpolateEvent onInterpolate;

		public override void Interpolate ( float t )
		{
			onInterpolate.Invoke( Vector2.Lerp( a, b, t ) );
		}
	}
}
