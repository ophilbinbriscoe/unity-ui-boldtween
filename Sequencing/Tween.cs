using System.Collections;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace BoldTween.Sequences
{
	public class Tween : Delay
	{
		[SerializeField]
		private AnimationCurve curve = AnimationCurve.Linear( 0.0f, 0.0f, 1.0f, 1.0f );

		[SerializeField]
		private TweenEvent onTween;

#if UNITY_EDITOR
		public override void OnValidate ()
		{
			onTween.OnValidate();

			curve.NormalizeDomain().NormalizeRange();
		}
#endif

		public override float Tick ( float deltaTime )
		{
			float overflow = base.Tick( deltaTime );

			onTween.Invoke( curve.Evaluate( elapsed / duration ) );

			return overflow;
		}
	}
}
