using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ToBoldlyPlay.Tweening
{
	public interface IInterpolator
	{
		void Interpolate ( float t );
	}
}