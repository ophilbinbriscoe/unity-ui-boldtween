using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace BoldTween
{
	[Serializable]
	[ExecuteInEditMode]
	public abstract class Interpolator : MonoBehaviour
	{
		[SerializeField]
		[Range( 0.0f, 1.0f )]
		protected float position;

		public void Interpolate ( float t )
		{
			if ( t != position )
			{
				position = t;

				Sync();
			}
		}

		protected virtual void OnEnable () { OnInterpolate( position ); }

		protected abstract void OnInterpolate ( float t );

		protected void Sync ()
		{
			if ( isActiveAndEnabled )
			{
				OnInterpolate( position );
			}
		}

		protected virtual void OnValidate ()
		{
			Sync();
		}
	}

	[Serializable]
	public abstract class Interpolator<T> : Interpolator
	{
		[SerializeField]
		protected T a, b;

		public virtual T A
		{
			get
			{
				return a;
			}

			set
			{
				a = value;

				Sync();
			}
		}

		public virtual T B
		{
			get
			{
				return b;
			}

			set
			{
				b = value;

				Sync();
			}
		}

		protected abstract T Interpolate ( T a, T b, float t );

		protected override void OnInterpolate ( float t )
		{
			OnInterpolate( Interpolate( a, b, t ) );
		}

		protected abstract void OnInterpolate ( T value );
	}
}
