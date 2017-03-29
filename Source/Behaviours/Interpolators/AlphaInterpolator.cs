using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;

namespace ToBoldlyPlay.Tweening
{
	[RequireComponent( typeof( CanvasGroup ) )]
	public class AlphaInterpolator : Interpolator
	{
		public CanvasGroup group;

		[SerializeField]
		[Range( 0.0f, 1.0f )]
		private float a = 0.0f, b = 1.0f;

		public override void Interpolate ( float t )
		{
			if ( group != null )
			{
				group.alpha = Mathf.Lerp( a, b, t );
			}
		}
	}
}
