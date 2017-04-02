using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ToBoldlyPlay.Tweening
{
	[Serializable]
	public struct EdgePadding
	{
		public float @in, @out;

		public EdgePadding ( float @in, float @out )
		{
			this.@in = @in;
			this.@out = @out;
		}
	}

	public class EdgeInterpolator : Interpolator, IRect
	{
		public RectTransform.Edge edge;

		public bool invert;

		public EdgePadding padding;

		//[Tooltip( "If true, the RectTransform will fly out to the opposite edge.")]
		//public bool across;

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
				if ( invert )
				{
					t = 1.0f - t;
				}

				float pad = Mathf.Lerp( -padding.@out, padding.@in, t );

				switch ( edge )
				{
				case RectTransform.Edge.Left:
					rect.pivot = Vector2.Lerp( new Vector2( 1.0f, rect.pivot.y ), new Vector2( 0.0f, rect.pivot.y ), t );
					rect.anchorMin = new Vector2( 0.0f, rect.anchorMin.y );
					rect.anchorMax = new Vector2( 0.0f, rect.anchorMax.y );
					rect.anchoredPosition = new Vector2( pad, rect.anchoredPosition.y );
					break;
				case RectTransform.Edge.Right:
					rect.pivot = Vector2.Lerp( new Vector2( 0.0f, rect.pivot.y ), new Vector2( 1.0f, rect.pivot.y ), t );
					rect.anchorMin = new Vector2( 1.0f, rect.anchorMin.y );
					rect.anchorMax = new Vector2( 1.0f, rect.anchorMax.y );
					rect.anchoredPosition = new Vector2( -pad, rect.anchoredPosition.y );
					break;
				case RectTransform.Edge.Top:
					rect.pivot = Vector2.Lerp( new Vector2( rect.pivot.x, 0.0f ), new Vector2( rect.pivot.x, 1.0f ), t );
					rect.anchorMin = new Vector2( rect.anchorMin.x, 1.0f );
					rect.anchorMax = new Vector2( rect.anchorMax.x, 1.0f );
					rect.anchoredPosition = new Vector2( rect.anchoredPosition.x, -pad );
					break;
				case RectTransform.Edge.Bottom:
					rect.pivot = Vector2.Lerp( new Vector2( rect.pivot.x, 1.0f ), new Vector2( rect.pivot.x, 0.0f ), t );
					rect.anchorMin = new Vector2( rect.anchorMin.x, 0.0f );
					rect.anchorMax = new Vector2( rect.anchorMax.x, 0.0f );
					rect.anchoredPosition = new Vector2( rect.anchoredPosition.x, pad );
					break;
				}
			}
		}

		private RectTransform.Edge Opposite ( RectTransform.Edge edge )
		{
			switch ( edge )
			{
			case RectTransform.Edge.Left:

				return RectTransform.Edge.Right;

			case RectTransform.Edge.Right:

				return RectTransform.Edge.Left;

			case RectTransform.Edge.Top:

				return RectTransform.Edge.Bottom;

			case RectTransform.Edge.Bottom:

				return RectTransform.Edge.Top;
			}

			throw new InvalidOperationException();
		}
	}
}
