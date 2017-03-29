using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;

namespace ToBoldlyPlay.Tweening
{
	[Serializable]
	public struct Anchor2D
	{
		public Vector2 min, max;

		public Anchor2D ( Vector2 min, Vector2 max )
		{
			this.min = min;
			this.max = max;
		}
	}

	public class AnchorInterpolator : Interpolator<Anchor2D>, IRect
	{
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
				rect.anchorMin = Vector2.Lerp( a.min, b.min, t );
				rect.anchorMax = Vector2.Lerp( a.max, b.max, t );
			}
		}
	}
}
