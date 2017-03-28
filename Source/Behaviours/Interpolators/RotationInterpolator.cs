using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;

namespace ToBoldlyPlay.Tweening
{
	public class RotationInterpolator : Interpolator<Quaternion>
	{
		[SerializeField]
		private new Transform transform;

		[SerializeField]
		private Space space;

		public override void Interpolate ( float t )
		{
			if ( transform != null )
			{
				switch ( space )
				{
				case Space.World:
					transform.rotation = Quaternion.Lerp( a, b, t );
					break;
				case Space.Self:
					transform.localRotation = Quaternion.Lerp( a, b, t );
					break;
				}
			}
		}
	}
}
