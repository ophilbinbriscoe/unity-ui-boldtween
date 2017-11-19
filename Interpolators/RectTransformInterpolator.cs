using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace BoldTween
{
	[Serializable]
	[RequireComponent( typeof( RectTransform ) )]
	public abstract class RectTransformInterpolator : ObjectBasedInterpolator<RectTransform>
	{
		protected DrivenRectTransformTracker tracker;

		public override RectTransform Target
		{
			get
			{
				return target;
			}

			set
			{
				tracker.Clear();

				target = value;

				Sync();
			}
		}

		protected virtual void OnDisable ()
		{
			tracker.Clear();
		}
	}

	[Serializable]
	[RequireComponent( typeof( RectTransform ) )]
	public abstract class RectTransformInterpolator<T> : ObjectBasedInterpolator<RectTransform, T>
	{
		protected DrivenRectTransformTracker tracker;

		public override RectTransform Target
		{
			get
			{
				return target;
			}

			set
			{
				tracker.Clear();

				target = value;

				Sync();
			}
		}

		protected virtual void OnDisable ()
		{
			tracker.Clear();
		}
	}
}
