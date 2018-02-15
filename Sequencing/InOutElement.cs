using System.Collections;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace BoldTween.Sequences
{
	public abstract class InOutElement : Delay
	{
		[SerializeField]
		private bool durationFromTweener;

		[SerializeField]
		protected InOut tweener;

		public override void OnValidate ()
		{
			base.OnValidate();

			if ( durationFromTweener && tweener != null )
			{
				duration = tweener.Duration;
			}
		}

		protected float GetDuration ()
		{
			if ( durationFromTweener && tweener != null )
			{
				return tweener.Duration;
			}
			else
			{
				return duration;
			}
		}
	}
}
