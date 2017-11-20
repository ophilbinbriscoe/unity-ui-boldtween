using System.Collections;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.Events;

namespace BoldTween.Sequences
{
	public class Sequence : MonoBehaviour
	{
		[SerializeField]
		private bool playOnAwake;

		[SerializeField]
		private bool runInFixedTime;

		[SerializeField]
		private bool runInUnscaledTime;

		[SerializeField]
		//[HideInInspector]
		private bool isPlaying;

		[SerializeField]
		//[HideInInspector]
		private int index;

		[SerializeField]
		private SequenceElement[] elements;

#if UNITY_EDITOR
		private void OnValidate ()
		{
			foreach ( var element in elements )
			{
				element.OnValidate();
			}
		}
#endif

		private void Awake ()
		{
			if ( playOnAwake )
			{
				Play();
			}
		}

		private void FixedUpdate ()
		{
			if ( !isPlaying )
			{
				return;
			}

			if ( !runInFixedTime )
			{
				return;
			}

			Tick( runInUnscaledTime ? Time.fixedUnscaledDeltaTime : Time.fixedDeltaTime );
		}

		private void Update ()
		{
			if ( !isPlaying )
			{
				return;
			}

			if ( runInFixedTime )
			{
				return;
			}

			Tick( runInUnscaledTime ? Time.unscaledDeltaTime : Time.deltaTime );
		}

		private void Tick ( float deltaTime )
		{
			if ( index < elements.Length )
			{
				var element = elements[index];

				float infiniteLoopCheck = deltaTime;

				do
				{
					deltaTime = element.Tick( deltaTime );

					if ( element.IsDone )
					{
						if ( ++index >= elements.Length )
						{
							isPlaying = false;

							return;
						}
						else
						{
							element = elements[index];
						}
					}
					else if ( deltaTime == infiniteLoopCheck )
					{
						Debug.LogErrorFormat( this, "A sequence element {0} may be causing an infinite loop.", element );

						return;
					}
				}
				while ( deltaTime > 0.0f );
			}
			else
			{
				isPlaying = false;
			}
		}

		public void Play ()
		{
			isPlaying = true;
		}

		public void Stop ()
		{
			isPlaying = false;
		}

		public void ResetSequence ( bool stop = false )
		{
			if ( stop )
			{
				Stop();
			}

			index = 0;
		}
	}
}
