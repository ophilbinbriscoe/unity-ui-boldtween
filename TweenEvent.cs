using System;
using UnityEngine;
using UnityEngine.Events;

namespace BoldTween
{
	[Serializable]
	public class FloatEvent : UnityEvent<float> { }

	[Serializable]
	public class TweenEvent
	{
		[SerializeField]
		private FloatEvent innerEvent;

		public UnityEvent<float> InnerEvent
		{
			get
			{
				return innerEvent;
			}
		}

		private bool HasListenerFor ( Interpolator interpolator )
		{
			int persistentEventCount = innerEvent.GetPersistentEventCount();

			for ( int i = 0; i < persistentEventCount; i++ )
			{
				if ( innerEvent.GetPersistentTarget( i ) == interpolator )
				{
					return true;
				}
			}

			return false;
		}

		public void Invoke ( float position )
		{
			innerEvent.Invoke( position );
		}

#if UNITY_EDITOR
		[SerializeField]
		private int persistentEventCount;

		[SerializeField]
		private Interpolator interpolatorToAdd;

		[SerializeField]
		private GameObject interpolatorsToAdd;

		public void OnValidate ()
		{
			if ( interpolatorToAdd != null )
			{
				if ( !HasListenerFor( interpolatorToAdd ) )
				{
					UnityEditor.Events.UnityEventTools.AddPersistentListener( innerEvent, interpolatorToAdd.Interpolate );
				}

				interpolatorToAdd = null;
			}

			if ( interpolatorsToAdd != null )
			{
				foreach ( var interpolator in interpolatorsToAdd.GetComponents<Interpolator>() )
				{
					if ( HasListenerFor( interpolator ) )
					{
						continue;
					}

					UnityEditor.Events.UnityEventTools.AddPersistentListener( innerEvent, interpolator.Interpolate );
				}

				interpolatorsToAdd = null;
			}

			int persistentListenerIndex = persistentEventCount;

			while ( persistentListenerIndex < innerEvent.GetPersistentEventCount() )
			{
				innerEvent.SetPersistentListenerState( persistentListenerIndex, UnityEventCallState.EditorAndRuntime );

				persistentListenerIndex++;
			}

			persistentEventCount = innerEvent.GetPersistentEventCount();
		}
#endif
	}
}
