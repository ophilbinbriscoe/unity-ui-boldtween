using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.Events;

namespace BoldTween
{
	[Serializable]
	public abstract class ObjectBasedInterpolator<O> : Interpolator, IObjectBasedInterpolator<O> where O : UnityEngine.Object
	{
		[SerializeField]
		protected O target;

		public virtual O Target
		{
			get
			{
				return target;
			}

			set
			{
				target = value;

				Sync();
			}
		}

		protected virtual void Reset ()
		{
			target = GetComponent<O>();
		}

		protected override void OnInterpolate ( float t )
		{
			if ( target == null )
			{
				return;
			}

			OnInterpolate( target, t );

#if UNITY_EDITOR
			if ( !Application.isPlaying )
			{
				UnityEditor.EditorUtility.SetDirty( target );
			}
#endif
		}

		protected abstract void OnInterpolate ( O target, float t );
	}

	[Serializable]
	public abstract class ObjectBasedInterpolator<O, T> : Interpolator<T>, IObjectBasedInterpolator<O> where O : UnityEngine.Object
	{
		[SerializeField]
		protected O target;

		public virtual O Target
		{
			get
			{
				return target;
			}

			set
			{
				target = value;

				Sync();
			}
		}

		protected virtual void Reset ()
		{
			target = GetComponent<O>();
		}

		protected override void OnInterpolate ( T value )
		{
			if ( target == null )
			{
				return;
			}

			OnInterpolate( target, value );

#if UNITY_EDITOR
			if ( !Application.isPlaying )
			{
				UnityEditor.EditorUtility.SetDirty( target );
			}
#endif
		}

		protected abstract void OnInterpolate ( O target, T value );
	}
}
