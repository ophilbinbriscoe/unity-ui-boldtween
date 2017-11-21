using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using System;

namespace BoldTween
{
	public class DelayedInterpolator : Interpolator
	{
		[SerializeField]
		private float delay;

		[SerializeField]
		private bool useUnscaledTime;

		private Queue<Vector2> buffer;

		[SerializeField]
		private TweenEvent onDelayedInterpolate;

		protected override void OnInterpolate ( float t )
		{
			buffer.Enqueue( new Vector2( useUnscaledTime ? Time.unscaledTime : Time.time, t ) );
		}

		private void Update ()
		{
			while ( buffer.Count > 0 )
			{
				while ( buffer.Peek().x + delay < (useUnscaledTime ? Time.unscaledTime : Time.time) )
				{
					onDelayedInterpolate.Invoke( buffer.Dequeue().y );
				}
			}
		}
	}
}
