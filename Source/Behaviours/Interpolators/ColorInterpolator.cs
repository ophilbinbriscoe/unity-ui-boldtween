using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

namespace ToBoldlyPlay.Tweening
{
	[Serializable]
	public class ColorInterpolator : Interpolator<Color>
	{
		[Serializable]
		public class InterpolateEvent : UnityEvent<Color> { }

		public InterpolateEvent onInterpolate;

		protected override Color DefaultA
		{
			get
			{
				return new Color( 1, 1, 1, 0.0f );
			}
		}

		protected override Color DefaultB
		{
			get
			{
				return Color.white;
			}
		}

		public override void Interpolate ( float t )
		{
			onInterpolate.Invoke( Color.Lerp( a, b, t ) );
		}
	}
}
