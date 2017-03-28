using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ToBoldlyPlay.Tweening
{
	[RequireComponent( typeof( RectTransform ) )]
	public class EdgeInterpolator : Interpolator
	{
		[SerializeField]
		private RectTransform.Edge edge;

		[SerializeField]
		[Tooltip( "If true, the RectTransform will fly out to the opposite edge.")]
		private bool across;

		[SerializeField]
		[HideInInspector]
		private RectTransform rect;

		private void Awake ()
		{
			rect = GetComponent<RectTransform>();
		}

		public override void Interpolate ( float t )
		{
			float dimension = rect.rect.height;

			if ( edge == RectTransform.Edge.Left || edge == RectTransform.Edge.Right )
			{
				dimension = rect.rect.width;
			}

			if ( across )
			{
				rect.SetInsetAndSizeFromParentEdge( Opposite( edge ), dimension * (t - 0.5f) * 2.0f, dimension );
			}
			else
			{
				rect.SetInsetAndSizeFromParentEdge( edge, dimension * (t - 1.0f), dimension );
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
