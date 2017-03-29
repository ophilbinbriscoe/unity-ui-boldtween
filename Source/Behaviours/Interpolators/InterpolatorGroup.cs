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

		public int Count
		{
			get
			{
				return interpolators.Count;
			}
		}

		public bool IsReadOnly
		{
			get
			{
				return false;
			}
		}

		public void Add<I> ( I item ) where I : UnityEngine.Object, IInterpolator
		{
			if ( !interpolators.Contains( item ) )
			{
				interpolators.Add( item );
			}
		}

		public void Clear ()
		{
			interpolators.Clear();
		}

		public bool Contains<I> ( I item ) where I : UnityEngine.Object, IInterpolator
		{
			return interpolators.Contains( item );
		}

		public override void Interpolate ( float t )
		{
			foreach ( var interpolator in interpolators )
			{
				(interpolator as IInterpolator).Interpolate( t );
			}
		}

		public bool Remove<I> ( I item ) where I : UnityEngine.Object, IInterpolator
		{
			return interpolators.Remove( item );
		}
	}
}