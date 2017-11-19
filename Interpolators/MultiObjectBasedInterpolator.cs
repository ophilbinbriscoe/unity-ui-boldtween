using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.Events;

namespace BoldTween
{
	[Serializable]
	public abstract class MultiObjectBasedInterpolator<O> : Interpolator, IMultiObjectBasedInterpolator<O> where O : UnityEngine.Object
	{
		[SerializeField]
		protected List<O> targets;

		private ReadOnlyCollection<O> targetsReadOnly;

		public virtual ReadOnlyCollection<O> Targets
		{
			get
			{
#if UNITY_EDITOR
				return targetsReadOnly == null ? targetsReadOnly= targets.AsReadOnly() : targetsReadOnly;
#else
				return targetsReadOnly;
#endif
			}
		}

		private void Awake ()
		{
			targetsReadOnly = targets.AsReadOnly();
		}

		public void Add ( O target )
		{
			if ( !targets.Contains( target ) )
			{
				targets.Add( target );

				Sync();
			}
		}

		public void Remove ( O target )
		{
			targets.Remove( target );
		} 

		protected override void OnInterpolate ( float t )
		{
			for ( int i = 0; i < targets.Count; i++ )
			{
#if UNITY_EDITOR
				if ( targets[i] == null )
				{
					Debug.LogError( "Interpolation target was null.", this );

					continue;
				}
#endif

				OnInterpolate( targets[i], t );

#if UNITY_EDITOR
				if ( !Application.isPlaying )
				{
					UnityEditor.EditorUtility.SetDirty( targets[i] );
				}
#endif
			}
		}

		protected abstract void OnInterpolate ( O target, float t );
	}

	[Serializable]
	public abstract class MultiObjectBasedInterpolator<O, T> : Interpolator<T>, IMultiObjectBasedInterpolator<O> where O : UnityEngine.Object
	{
		[SerializeField]
		protected List<O> targets;

		private ReadOnlyCollection<O> targetsReadOnly;

		public virtual ReadOnlyCollection<O> Targets
		{
			get
			{
#if UNITY_EDITOR
				return targetsReadOnly == null ? targetsReadOnly = targets.AsReadOnly() : targetsReadOnly;
#else
				return targetsReadOnly;
#endif
			}
		}

		private void Awake ()
		{
			targetsReadOnly = targets.AsReadOnly();
		}

		public void Add ( O target )
		{
			if ( !targets.Contains( target ) )
			{
				targets.Add( target );

				Sync();
			}
		}

		public void Remove ( O target )
		{
			targets.Remove( target );
		}

		protected override void OnInterpolate( T value )
		{
			for ( int i = 0; i < targets.Count; i++ )
			{
#if UNITY_EDITOR
				if ( targets[i] == null )
				{
					Debug.LogError( "Interpolation target was null.", this );

					continue;
				}
#endif

				OnInterpolate( targets[i], value );

#if UNITY_EDITOR
				if ( !Application.isPlaying )
				{
					UnityEditor.EditorUtility.SetDirty( targets[i] );
				}
#endif
			}
		}

		protected abstract void OnInterpolate ( O target, T value );
	}
}
