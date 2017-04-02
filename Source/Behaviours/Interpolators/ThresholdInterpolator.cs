using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

namespace ToBoldlyPlay.Tweening
{
	public class ThresholdInterpolator : Interpolator
	{
		[Serializable]
		public class ThresholdEvent : UnityEvent<bool> { }

		[SerializeField]
		[Range( 0.0f, 1.0f )]
		private float threshold;

		public float Threshold
		{
			get
			{
				return threshold;
			}

			set
			{
				threshold = Mathf.Clamp01( value );
			}
		}

		public UnityEvent onTrue, onFalse;
		public ThresholdEvent onCross;

		private bool init;
		private bool current;

		public override void Interpolate ( float t )
		{
			bool value = threshold == 0.0f ? t > threshold : t >= threshold;

			if ( current != value || !init )
			{
				init = true;

				onCross.Invoke( current = value );

				if ( current )
				{
					onTrue.Invoke();
				}
				else
				{
					onFalse.Invoke();
				}
			}
		}
	}
}
