using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;

namespace ToBoldlyPlay.Tweening
{
	public class PositionInterpolator : Interpolator<Vector3>
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
					transform.position = Vector3.Lerp( a, b, t );
					break;
				case Space.Self:
					transform.localPosition = Vector3.Lerp( a, b, t );
					break;
				}
			}
		}
	}
}
