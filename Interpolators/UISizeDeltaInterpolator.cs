using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;

namespace BoldTween.UI
{
	public class UISizeDeltaInterpolator : RectTransformInterpolator<float>
	{
		[SerializeField]
		private RectTransform.Axis axis;

		[SerializeField]
		[HideInInspector]
		private RectTransform.Axis activeAxis;

#if UNITY_EDITOR
		protected override void Reset ()
		{
			base.Reset();

			activeAxis = axis = RectTransform.Axis.Horizontal;

			a = target.sizeDelta.x;
			b = target.sizeDelta.x;
		}

		protected override void OnValidate ()
		{
			if ( axis != activeAxis )
			{
				switch ( axis )
				{
				case RectTransform.Axis.Horizontal:
					a = target.sizeDelta.x;
					b = target.sizeDelta.x;
					break;

				case RectTransform.Axis.Vertical:
					a = target.sizeDelta.y;
					b = target.sizeDelta.y;
					break;
				}

				activeAxis = axis;
			}

			base.OnValidate();
		}
#endif

		protected override void OnInterpolate ( RectTransform target, float value )
		{
			tracker.Clear();

			switch ( axis )
			{
			case RectTransform.Axis.Horizontal:
				tracker.Add( this, target, DrivenTransformProperties.SizeDeltaX );
				break;

			case RectTransform.Axis.Vertical:
				tracker.Add( this, target, DrivenTransformProperties.SizeDeltaY );
				break;
			}

			target.SetSizeWithCurrentAnchors( axis, value );
		}

		protected override float Interpolate ( float a, float b, float t )
		{
			return a + (b - a) * t;
		}
	}
}