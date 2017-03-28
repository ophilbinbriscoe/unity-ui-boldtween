using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;

namespace ToBoldlyPlay.Tweening
{
	public class TwistInterpolator : Interpolator<float>
	{
		[SerializeField]
		private RectTransform rect;

		public override void Interpolate ( float t )
		{
			if ( rect != null )
			{
				rect.localRotation = Quaternion.Euler( 0.0f, 0.0f, Mathf.Lerp( a, b, t ) );
			}
		}
	}
}
