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
		[AssetField]
#endif
		[Tooltip( "Defines a curve and duration." )]
		public TweenPreset preset;

		[SerializeField]
		private AnimationCurve curve = AnimationCurve.EaseInOut( 0.0f, 0.0f, 1.0f, 1.0f );

		[SerializeField]
		[Range( 0.0f, float.MaxValue )]
		private float duration = 1.0f;

		[SerializeField]
		[EnumField( EnumStyle.Mask )]
		private ModifierFlags modifiers;

		[SerializeField]
		[Range( 0.0f, float.MaxValue )]
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

		private float DeltaTime
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

		[Tooltip( "Sibling Tweeners will have their tweens interrupted any time this Tweener starts a new tween." )]
		public List<Tweener> siblings = new List<Tweener>( 0 );

		[Tooltip( "Invoked every time a new Tween is started.")]
		public UnityEvent onTweenStart;

		[Tooltip( "Invoked every time a Tween finishes or is interrupted.")]
		public UnityEvent onTweenEnd;

		[Tooltip( "Invoked every frame that a Tween is in progress.")]
		public TweenEvent onTweenUpdate;

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
					Tween( preset.Curve, preset.Duration * multiplier, modifiers );
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
			Tween( preset.Curve, preset.Duration );
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
			float start = Time.time, time = start;

			/// Cache evaluated flags
			bool reverse = (modifiers & ModifierFlags.Reverse) != 0;
			bool invert = (modifiers & ModifierFlags.Invert) != 0;
			bool min = (modifiers & ModifierFlags.Min) != 0;
			bool max = (modifiers & ModifierFlags.Max) != 0;
			//bool debug = (modifiers & ModifierFlags.Debug) != 0;

			/// Infinite loop guard
			while ( DeltaTime == 0.0f )
			{
				yield return null;
			}

			if ( min )
			{
				bool done = false;

				float t = Calculate( 0.0f, curve, reverse, invert, false, false, ref done );

				/// Simulate Tween until t is lesser or equal to the current position
				while ( t > position && !done )
				{
					/// Increment
					time += DeltaTime;

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
					time += DeltaTime;

					t = Calculate( (time - start) / duration, curve, reverse, invert, false, false, ref done );
				}
			}

			onTweenStart.Invoke();

			/// Tween
			{
				bool done = false;

				float t = 0.0f;
				if ( duration > 0.0f )
				{
					while ( !done )
					{
						t = (time - start) / duration;

						t = Calculate( t, curve, reverse, invert, min, max, ref done );

						/// Apply Tween
						Interpolate( t );

						onTweenUpdate.Invoke( t );

						/// Increment time
						time += DeltaTime;

						yield return null;
					}
				}
				else
				{
					t = Calculate( 1.0f, curve, reverse, invert, min, max, ref done );

					Interpolate( t );

					onTweenUpdate.Invoke( t );
				}
			}

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
