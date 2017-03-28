using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ToBoldlyPlay.Tweening
{
	public class CenterInterpolator : Interpolator
	{
		[SerializeField]
		private RectTransform.Edge edge;

		[SerializeField]
		private RectTransform rect;

		public override void Interpolate ( float t )
		{
			if ( rect != null )
			{
				float dimension = rect.rect.height;

				if ( edge == RectTransform.Edge.Left || edge == RectTransform.Edge.Right )
				{
					dimension = rect.rect.width;
				}

				float parent = (rect.parent as RectTransform).rect.height;

				if ( edge == RectTransform.Edge.Left || edge == RectTransform.Edge.Right )
				{
					parent = (rect.parent as RectTransform).rect.width;
				}

				rect.SetInsetAndSizeFromParentEdge( edge, Mathf.Lerp( dimension, (parent - dimension), t ) * (t * 1.5f - 1.0f), dimension );
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
