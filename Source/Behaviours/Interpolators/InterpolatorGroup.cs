using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ToBoldlyPlay.Tweening
{
	public class InterpolatorGroup : Interpolator
	{
		[SerializeField]
		[ReferenceField( allowSceneObjects = true, type = typeof( IInterpolator ) )]
		private List<UnityEngine.Object> interpolators = new List<UnityEngine.Object>( 1 );

		public override void Interpolate ( float t )
		{
			foreach ( var interpolator in interpolators )
			{
				(interpolator as IInterpolator).Interpolate( t );
			}
		}
	}
}