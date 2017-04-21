using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ToBoldlyPlay.Tweening
{
	public abstract class Interpolator : BoldBehaviour, IInterpolator
	{
		public abstract void Interpolate ( float t );
	}

	public abstract class Interpolator<T> : Interpolator
	{
		[SerializeField]
		protected T a;

		public T A
		{
			get
			{
				return a;
			}

			set
			{
				a = value;
			}
		}

		[SerializeField]
		protected T b;

		public T B
		{
			get
			{
				return b;
			}

			set
			{
				b = value;
			}
		}

		protected virtual T DefaultA
		{
			get
			{
				return default( T );
			}
		}

		protected virtual T DefaultB
		{
			get
			{
				return default( T );
			}
		}

		private void Reset ()
		{
			a = DefaultA;
			b = DefaultB;
		}
	}
}