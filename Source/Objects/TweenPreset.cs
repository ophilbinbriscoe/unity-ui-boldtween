using System.Collections;
using System.Collections.Generic;

using UnityEngine;

namespace ToBoldlyPlay.Tweening
{
	[CreateAssetMenu( fileName = "new TweenPreset.asset", menuName = "Tween Preset", order = 411 )]
	public class TweenPreset : ScriptableObject
	{
		[SerializeField]
		private AnimationCurve curve = AnimationCurve.EaseInOut( 0.0f, 0.0f, 1.0f, 1.0f );

		public AnimationCurve Curve
		{
			get
			{
				return curve;
			}
		}

		[SerializeField]
		[Range( 0.0f, float.MaxValue )]
		private float duration = 1.0f;

		public float Duration
		{
			get
			{
				return duration;
			}
		}
	}
}
