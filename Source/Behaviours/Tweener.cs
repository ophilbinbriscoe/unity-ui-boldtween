using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine;
using UnityEngine.Events;

namespace ToBoldlyPlay.Tweening
{
	using Events;

	public partial class Tweener :
#if BOLD_EVENT
		EventObjectHandler, 
#else
		MonoBehaviour,
#endif
		IInterpolator
	{
		[SerializeField]
		private TweenType type = TweenType.Preset;

#if BOLD_EDITOR
		[ReferenceField( allowSceneObjects = false, enumerate = true )]
#endif
		[Tooltip( "Defines a curve and duration." )]
		public TweenPreset preset;

		[SerializeField]
		private AnimationCurve curve = AnimationCurve.EaseInOut( 0.0f, 0.0f, 1.0f, 1.0f );

		[SerializeField]
		private float duration = 1.0f;

		[SerializeField]
		[EnumField( EnumStyle.Mask )]
		private ModifierFlags modifiers;

		[SerializeField]
		[Tooltip( "The preset duration is multiplied with this value." )]
		private float multiplier = 1.0f;

		public float Multiplier
		{
			get
			{
				return multiplier;
			}

			set
			{
				multiplier = Mathf.Clamp( value, float.Epsilon, float.MaxValue );
			}
		}

		[SerializeField]
		private InitializationType initialize = InitializationType.OnAwake;

		[SerializeField]
		[Range( 0.0f, 1.0f )]
		private float initialPosition = 0.0f;

		[SerializeField]
		[HideInInspector]
		private float position;

		public float Position
		{
			get
			{
				return position;
			}

			set
			{
				
			}
		}

		public bool IsTweening
		{
			get
			{
				return coroutine != null;
			}
		}

		[SerializeField]
		private TimeType timeType = TimeType.Unscaled;

		private float LocalTime
		{
			get
			{
				return timeType == TimeType.Unscaled ? Time.unscaledTime : Time.time;
			}
		}

		private float LocalDeltaTime
		{
			get
			{
				return timeType == TimeType.Unscaled ? Time.unscaledDeltaTime : Time.deltaTime;
			}
		}

		[SerializeField]
		[HideInInspector]
		private Coroutine coroutine;

		[SerializeField]
		[ReferenceField( allowSceneObjects = true, type = typeof( IInterpolator ) )]
		private List<UnityEngine.Object> interpolators = new List<UnityEngine.Object>( 1 );

		private List<IInterpolator> interfaces = new List<IInterpolator>();

		public void AddInterpolator ( IInterpolator interpolator )
		{
			if ( interpolator is Object )
			{
				interpolators.Add( interpolator as Object );
			}
			else
			{
				interfaces.Add( interpolator );
			}
		}

		public bool RemoveInterpolator ( IInterpolator interpolator )
		{
			if ( interpolator is Object )
			{
				return interpolators.Remove( interpolator as Object );
			}
			else
			{
				return interfaces.Remove( interpolator );
			}
		}

		[Tooltip( "Sibling Tweeners will have their tweens interrupted any time this Tweener starts a new tween." )]
		public List<Tweener> siblings = new List<Tweener>( 0 );

		[Tooltip( "Invoked every time a new Tween is started.")]
		public UnityEvent onTweenStart;

		[Tooltip( "Invoked every time a Tween finishes or is interrupted.")]
		public UnityEvent onTweenEnd;

		[Tooltip( "Invoked every frame that a Tween is in progress.")]
		public TweenEvent onTweenUpdate;

		private bool isPaused;

		public bool IsPaused
		{
			get
			{
				return isPaused;
			}
		}

		public void Pause ()
		{
			isPaused = true;
		}

		public void Resume ()
		{
			isPaused = false;
		}

#if BOLD_EVENT
		protected override void OnEnable ()
		{
			if ( initialize == InitializationType.OnEnable )
			{
				Interpolate( initialPosition );
			}

			base.OnEnable();
		}

		protected override void HandleInvoke ( EventObject @event )
		{
			Tween();
		}
#else
		private void OnEnable ()
		{
			if ( initialize == InitializationType.OnEnable )
			{
				Interpolate( initialPosition );
			}
		}
#endif

		private void Awake ()
		{
			if ( initialize == InitializationType.OnAwake )
			{
				Interpolate( initialPosition );
			}
		}

		private void Start ()
		{
			if ( initialize == InitializationType.OnStart )
			{
				Interpolate( initialPosition );
			}
		}

		public void Tween ()
		{
			switch ( type )
			{
			case TweenType.Custom:
				Tween( curve, duration, modifiers );
				break;

			case TweenType.Preset:
				if ( preset != null )
				{
					Tween( preset.Curve, preset.Duration * multiplier, preset.Modifiers );
				}
				else
				{
					Debug.LogWarning( "No TweenPreset found, please assign one in the inspector.", this );
				}
				break;
			}
		}

		public void Tween ( TweenPreset preset )
		{
			Tween( preset.Curve, preset.Duration, preset.Modifiers );
		}

		public void Tween ( TweenPreset preset, float multiplier )
		{
			Tween( preset.Curve, preset.Duration * multiplier, preset.Modifiers );
		}

		public void Tween ( AnimationCurve curve, float duration, ModifierFlags modifiers = 0 )
		{
			Stop();

			foreach ( var sibling in siblings )
			{
				if ( sibling != null )
				{
					sibling.Stop();
				}
			}

			isPaused = false;

			coroutine = StartCoroutine( Coroutine( curve, duration, modifiers ) );
		}

		public void Stop ()
		{
			if ( coroutine != null )
			{
				StopCoroutine( coroutine );

				coroutine = null;

				onTweenEnd.Invoke();
			}
		}

		public void Interpolate ( float t )
		{
			position = t;

			foreach ( var interpolator in interpolators )
			{
				(interpolator as IInterpolator).Interpolate( t );
			}
		}

		public void Toggle ( ModifierFlags modifier )
		{
			modifiers = (modifiers & modifier) > 0 ? modifiers & ~modifier : modifiers | modifier; 
		}

		public void ToggleMinMaxReverse ()
		{
			ToggleReverse();
			ToggleMinMax();
		}

		public void ToggleReverse ()
		{
			Toggle( ModifierFlags.Reverse );
		}

		public void ToggleMinMax ()
		{
			Toggle( ModifierFlags.Min );
			Toggle( ModifierFlags.Max );
		}
		
		private IEnumerator Coroutine ( AnimationCurve curve, float duration, ModifierFlags modifiers )
		{
			/// Timestamp
			float start = LocalTime, time = start;

			/// Cache evaluated flags
			bool min = (modifiers & ModifierFlags.Min) != 0;
			bool max = (modifiers & ModifierFlags.Max) != 0;
			bool reverse = (modifiers & ModifierFlags.Reverse) != 0;
			bool invert = (modifiers & ModifierFlags.Invert) != 0;
			bool loop = (modifiers & ModifierFlags.Loop) != 0;
			bool ping = (modifiers & ModifierFlags.PingPong) != 0, pong = false;
			//bool debug = (modifiers & ModifierFlags.Debug) != 0;

			/// Infinite loop guard
			while ( LocalDeltaTime == 0.0f )
			{
				yield return null;
			}

			if ( duration > 0.0f )
			{
				if ( min )
				{
					bool done = false;

					float t = Calculate( 0.0f, curve, reverse, invert, false, false, ref done );

					/// Simulate Tween until t is lesser or equal to the current position
					while ( t > position && !done )
					{
						/// Increment
						time += LocalDeltaTime;

						t = Calculate( (time - start) / duration, curve, reverse, invert, false, false, ref done );
					}
				}

				if ( max )
				{
					bool done = false;

					float t = Calculate( 0.0f, curve, reverse, invert, false, false, ref done );

					/// Simulate Tween until t is greater or equal to the current position
					while ( t < position && !done )
					{
						/// Increment
						time += LocalDeltaTime;

						t = Calculate( (time - start) / duration, curve, reverse, invert, false, false, ref done );
					}
				}
			}

			onTweenStart.Invoke();

			/// Tween
			{
				Start:

				bool done = false;

				float t = 0.0f;

				if ( duration > 0.0f )
				{
					while ( !done )
					{
						if ( !isPaused )
						{
							t = (time - start) / duration;

							t = Calculate( t, curve, reverse, invert, min, max, ref done );

							/// Apply Tween
							Interpolate( t );

							onTweenUpdate.Invoke( t );

							/// Increment time
							time += LocalDeltaTime;
						}

						yield return null;
					}
				}
				else
				{
					t = Calculate( 1.0f, curve, reverse, invert, min, max, ref done );

					Interpolate( t );

					onTweenUpdate.Invoke( t );
				}

				if ( ping )
				{
					ping = false;
					pong = true;

					reverse = !reverse;

					goto Restart;
				}

				if ( loop )
				{
					if ( pong )
					{
						pong = false;
						ping = true;

						reverse = !reverse;
					}

					goto Restart;
				}

				goto Finish;

				Restart:

				start = LocalTime;
				time = start;

				goto Start;
			}

			Finish:

			/// Cleanup
			coroutine = null;

			onTweenEnd.Invoke();
		}

		private float Calculate ( float t, AnimationCurve curve, bool reverse, bool invert, bool min, bool max, ref bool done )
		{
			if ( t >= 1.0f )
			{
				t = 1.0f;

				done = true;
			}

			if ( reverse )
			{
				t = 1.0f - t;
			}

			t = curve.Evaluate( t );

			if ( invert )
			{
				t = 1.0f - t;
			}

			if ( min )
			{
				t = Mathf.Min( position, t );
			}

			if ( max )
			{
				t = Mathf.Max( position, t );
			}

			return t;
		}
	}
}
