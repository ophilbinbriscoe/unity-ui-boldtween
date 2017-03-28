using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

namespace ToBoldlyPlay.Tweening
{
	public class Vector3Interpolator : Interpolator<Vector3>
	{
		[Serializable]
		public class InterpolateEvent : UnityEvent<Vector3> { }

		[SerializeField]
		public InterpolateEvent onInterpolate;

		public override void Interpolate ( float t )
		{
			onInterpolate.Invoke( Vector3.Lerp( a, b, t ) );
		}
	}
}
