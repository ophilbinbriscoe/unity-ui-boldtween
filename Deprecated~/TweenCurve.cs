using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace BoldTween
{
	//[CreateAssetMenu( menuName = "new TweenCurve.asset", )]
	public class TweenCurve : ScriptableObject
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

		public float Evaluate ( float t )
		{
			return curve.Evaluate( t );
		}

		public float Evaluate ( float t, bool reverse, bool invert )
		{
			if ( reverse )
			{
				t = 1.0f - t;
			}

			t = Evaluate( t );

			if ( invert )
			{
				t = 1.0f - t;
			}

			return t;
		}

		public void SetFrom ( AnimationCurve curve )
		{
			UnityEngine.Assertions.Assert.IsTrue( curve.keys.Length > 1, "A TweenCurve must have at least 2 points." );

			this.curve.keys = new Keyframe[curve.keys.Length];

			for ( int i = 0; i < curve.keys.Length; i++ )
			{
				this.curve.keys[i] = curve.keys[i];
			}

			this.curve.NormalizeDomain().NormalizeRange();
		}
	}
}
