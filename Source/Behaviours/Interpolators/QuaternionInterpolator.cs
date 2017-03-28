using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

namespace ToBoldlyPlay.Tweening
{
	public class QuaternionInterpolator : Interpolator<Quaternion>
	{
		[Serializable]
		public class InterpolateEvent : UnityEvent<Quaternion> { }

		[SerializeField]
		public InterpolateEvent onInterpolate;

		public override void Interpolate ( float t )
		{
			onInterpolate.Invoke( Quaternion.Lerp( a, b, t ) );
		}
	}
}
