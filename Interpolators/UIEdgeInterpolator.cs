using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BoldTween.UI
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

	public enum Edge
	{
		None = 0,
		Left = 1,
		Right = 2,
		Top = 3,
		Bottom = 4
	}

	public class UIEdgeInterpolator : RectTransformInterpolator
	{
		[SerializeField]
		private bool invert;

		[SerializeField]
		private Edge edge;

		public Edge Edge
		{
			get
			{
				return edge;
			}

			set
			{
				edge = value;

				Sync();
			}
		}

		[SerializeField]
		private EdgePadding padding;

		public EdgePadding Padding
		{
			get
			{
				return padding;
			}

			set
			{
				padding = value;

				Sync();
			}
		}

#if UNITY_EDITOR
		[SerializeField]
		[HideInInspector]
		private Edge activeEdge;

		protected override void OnValidate ()
		{
			if ( activeEdge == Edge.None )
			{
				activeEdge = edge;

				switch ( edge )
				{
				case Edge.Left:
					position = target.pivot.x;
					break;
				case Edge.Right:
					position = 1.0f - target.pivot.x;
					break;
				case Edge.Top:
					position = target.pivot.y;
					break;
				case Edge.Bottom:
					position = 1.0f - target.pivot.y;
					break;
				}
			}

			base.OnValidate();
		}
#endif

		protected override void OnInterpolate ( RectTransform rect, float t )
		{
			tracker.Clear();

			if ( edge == Edge.None )
			{
				return;
			}

			if ( invert )
			{
				t = 1.0f - t;
			}

			float pad = OverLerp( -padding.@out, padding.@in, t );


			if ( edge < Edge.Top )
			{
				tracker.Add( this, rect, DrivenTransformProperties.PivotX );
				tracker.Add( this, rect, DrivenTransformProperties.AnchorMinX );
				tracker.Add( this, rect, DrivenTransformProperties.AnchorMaxX );
				tracker.Add( this, rect, DrivenTransformProperties.AnchoredPositionX );
			}
			else
			{
				tracker.Add( this, rect, DrivenTransformProperties.PivotY );
				tracker.Add( this, rect, DrivenTransformProperties.AnchorMinY );
				tracker.Add( this, rect, DrivenTransformProperties.AnchorMaxY );
				tracker.Add( this, rect, DrivenTransformProperties.AnchoredPositionY );
			}

			switch ( edge )
			{
			case Edge.Left:

				rect.pivot = OverLerp( new Vector2( 1.0f, rect.pivot.y ), new Vector2( 0.0f, rect.pivot.y ), t );
				rect.anchorMin = new Vector2( 0.0f, rect.anchorMin.y );
				rect.anchorMax = new Vector2( 0.0f, rect.anchorMax.y );
				rect.anchoredPosition = new Vector2( pad, rect.anchoredPosition.y );
				break;

			case Edge.Right:

				rect.pivot = OverLerp( new Vector2( 0.0f, rect.pivot.y ), new Vector2( 1.0f, rect.pivot.y ), t );
				rect.anchorMin = new Vector2( 1.0f, rect.anchorMin.y );
				rect.anchorMax = new Vector2( 1.0f, rect.anchorMax.y );
				rect.anchoredPosition = new Vector2( -pad, rect.anchoredPosition.y );
				break;

			case Edge.Top:

				rect.pivot = OverLerp( new Vector2( rect.pivot.x, 0.0f ), new Vector2( rect.pivot.x, 1.0f ), t );
				rect.anchorMin = new Vector2( rect.anchorMin.x, 1.0f );
				rect.anchorMax = new Vector2( rect.anchorMax.x, 1.0f );
				rect.anchoredPosition = new Vector2( rect.anchoredPosition.x, -pad );
				break;

			case Edge.Bottom:

				rect.pivot = OverLerp( new Vector2( rect.pivot.x, 1.0f ), new Vector2( rect.pivot.x, 0.0f ), t );
				rect.anchorMin = new Vector2( rect.anchorMin.x, 0.0f );
				rect.anchorMax = new Vector2( rect.anchorMax.x, 0.0f );
				rect.anchoredPosition = new Vector2( rect.anchoredPosition.x, pad );
				break;
			}
		}

		private float OverLerp ( float a, float b, float t )
		{
			if ( t == 0.0f )
			{
				return a;
			}

			if ( t == 1.0f )
			{
				return b;
			}

			return a += (b - a) * t;
		}

		private Vector2 OverLerp ( Vector2 a, Vector2 b, float t )
		{
			if ( t == 0.0f )
			{
				return a;
			}

			if ( t == 1.0f )
			{
				return b;
			}

			return a += (b - a) * t;
		}
	}
}
