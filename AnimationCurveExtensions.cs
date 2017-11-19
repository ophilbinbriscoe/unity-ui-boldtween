using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace BoldTween
{
	public static class AnimationCurveExtensions
	{
		public static AnimationCurve NormalizeDomain ( this AnimationCurve curve )
		{
			UnityEngine.Assertions.Assert.IsTrue( curve.keys.Length > 1, "Cannot normalize a curve with less than 2 points." );

			float x0 = curve.keys[0].time;
			float x1 = curve.keys[curve.keys.Length - 1].time;

			float dx = x1 - x0;

			for ( int i = 0; i < curve.keys.Length; i++ )
			{
				curve.keys[i].time -= x0;
				curve.keys[i].time /= dx;
			}

			return curve;
		}

		public static AnimationCurve NormalizeRange ( this AnimationCurve curve )
		{
			UnityEngine.Assertions.Assert.IsTrue( curve.keys.Length > 1, "Cannot normalize a curve with less than 2 points." );

			float y0 = curve.keys[0].time;
			float y1 = curve.keys[curve.keys.Length - 1].time;

			float dy = y1 - y0;

			for ( int i = 0; i < curve.keys.Length; i++ )
			{
				curve.keys[i].value -= y0;
				curve.keys[i].value /= dy;
			}

			return curve;
		}


		public static float Evaluate ( this AnimationCurve curve, float time, bool reverse, bool invert )
		{
			if ( reverse )
			{
				time = 1.0f - time;
			}

			time = curve.Evaluate( time );

			if ( invert )
			{
				time = 1.0f - time;
			}

			return time;
		}
	}
}
