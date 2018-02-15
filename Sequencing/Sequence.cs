using System.Collections;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.Events;
using System;

#if UNITY_EDITOR
using UnityEditor;
#endif

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
		private bool waitForGoodDeltaTime;

		[SerializeField]
		private float goodDeltaTime = 1.0f / 10.0f;

		[SerializeField]
		[HideInInspector]
		private bool hasGoodDeltaTime;

#if UNITY_EDITOR
		[SerializeField]
		private bool lockReloadingAssemblies;

		[SerializeField]
		[HideInInspector]
		private bool lockedReloadingAssemblies;
#endif

		[SerializeField]
		[HideInInspector]
		private bool isPlaying;

		[SerializeField]
		[HideInInspector]
		private int index;

		[SerializeField]
		private SequenceElement[] elements;

#if UNITY_EDITOR
		private void OnValidate ()
		{
			foreach ( var element in elements )
			{
				if ( element == null )
				{
					Debug.LogError( "Null element.", this );

					continue;
				}

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
			if ( waitForGoodDeltaTime && !hasGoodDeltaTime )
			{
				return;
			}

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
			//EvaluateDeltaTime();

			if ( waitForGoodDeltaTime && !hasGoodDeltaTime )
			{
				return;
			}

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

		private bool EvaluateDeltaTime ()
		{
			if ( !hasGoodDeltaTime )
			{
				Debug.LogFormat( "{0} vs {1}", Time.deltaTime, goodDeltaTime );

				if ( Time.deltaTime <= goodDeltaTime )
				{
					return hasGoodDeltaTime = true;
				}
				else if ( waitForGoodDeltaTime )
				{
					return false;
				}
			}

			return true;
		}

		private void Tick ( float deltaTime )
		{
			if ( index < elements.Length )
			{
				var element = elements[index];

#if UNITY_EDITOR
				Break( element );
#endif

				float infiniteLoopCheck = deltaTime;

				do
				{
					deltaTime = element.Tick( deltaTime );

					if ( element.IsDone )
					{
						if ( ++index >= elements.Length )
						{
							ResetSequence( true );

							return;
						}
						else
						{
							element = elements[index];

#if UNITY_EDITOR
							Break( element );
#endif
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
				ResetSequence( true );
			}
		}

#if UNITY_EDITOR
		private void Break ( SequenceElement element )
		{
			if ( element.Break )
			{
				Debug.Break();
			}
		}
#endif

		private class WaitForSequence : CustomYieldInstruction
		{
			private readonly Sequence sequence;

			public override bool keepWaiting
			{
				get
				{
					return sequence.isPlaying;
				}
			}

			public WaitForSequence ( Sequence sequence )
			{
				this.sequence = sequence;
			}
		}

		public void PlayWithoutYieldInstruction ()
		{
			Play();
		}

		public CustomYieldInstruction Play ()
		{
			isPlaying = true;

#if UNITY_EDITOR
			if ( lockReloadingAssemblies )
			{
				EditorApplication.LockReloadAssemblies();

				lockedReloadingAssemblies = true;
			}
#endif

			return new WaitForSequence( this );
		}

		public void Stop ()
		{
			isPlaying = false;

#if UNITY_EDITOR
			if ( lockedReloadingAssemblies )
			{
				EditorApplication.UnlockReloadAssemblies();

				lockedReloadingAssemblies = false;
			}
#endif
		}

		public void ResetSequence ( bool stop = false )
		{
			if ( stop )
			{
				Stop();
			}

			index = 0;

			foreach ( var element in elements )
			{
				element.Reset();
			}
		}
	}
}
