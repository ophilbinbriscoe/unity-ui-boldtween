using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;

namespace ToBoldlyPlay.Tweening
{
	public class ScaleInterpolator : Interpolator<Vector3>
	{
		[SerializeField]
		private new Transform transform;

		protected override Vector3 DefaultA
		{
			get
			{
				return Vector3.zero;
			}
		}

		protected override Vector3 DefaultB
		{
			get
			{
				return Vector3.one;
			}
		}

		public override void Interpolate ( float t )
		{
			if ( transform != null )
			{
				Vector3 scale = Vector3.Lerp( a, b, t );

				transform.localScale = new Vector3( scale.x, scale.y, scale.z );
			}
		}
	}
}