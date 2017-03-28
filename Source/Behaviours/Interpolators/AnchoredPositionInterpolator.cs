using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;

namespace ToBoldlyPlay.Tweening
{
	public class AnchoredPositionInterpolator : Interpolator<Vector2>
	{
		[SerializeField]
		private RectTransform rect;

		public override void Interpolate ( float t )
		{
			if ( rect != null )
			{
				rect.anchoredPosition = Vector2.Lerp( a, b, t );
			}
		}
	}
}
