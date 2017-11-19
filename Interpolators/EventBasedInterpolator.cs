using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.Events;

namespace BoldTween
{
	[Serializable]
	public abstract class EventBasedInterpolator<T> : Interpolator<T>
	{
		[Serializable]
		public class InterpolatorEvent : UnityEvent<T> { }

		[SerializeField]
		private InterpolatorEvent onInterpolate;

		protected override void OnInterpolate ( T value )
		{
			onInterpolate.Invoke( value );
		}
	}
}
