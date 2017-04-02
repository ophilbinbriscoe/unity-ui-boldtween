using System.Collections;
using System.Collections.Generic;

using UnityEngine;

namespace ToBoldlyPlay.Tweening
{
	[CreateAssetMenu( fileName = "new TweenPreset.asset", menuName = "Tween Preset", order = Const.ASSET_MENU_ORDER )]
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
