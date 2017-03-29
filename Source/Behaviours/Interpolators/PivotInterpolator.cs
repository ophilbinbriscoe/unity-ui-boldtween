using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;

namespace ToBoldlyPlay.Tweening
{
	public class PivotInterpolator : Interpolator<Vector2>, IRect
	{
		public bool positionStays;

		[SerializeField]
		private RectTransform rect;
		
		public RectTransform Rect
		{
			get
			{
				return rect;
			}

			set
			{
				rect = value;
			}
		}

		public override void Interpolate ( float t )
		{
			if ( rect != null )
			{
				if ( positionStays )
				{
					Vector2 pivot = Vector2.Lerp( a, b, t );

					Vector2 delta = pivot - rect.pivot;

					rect.pivot = pivot;

					Vector2 size = rect.rect.size;

					delta.Set( delta.x * size.x, delta.y * size.y );

					rect.anchoredPosition += delta;
				}
				else
				{
					rect.pivot = Vector2.Lerp( a, b, t );
				}
			}
		}
	}
}
