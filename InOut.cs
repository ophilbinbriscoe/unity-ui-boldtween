using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace BoldTween
{
	public enum Direction
	{
		None,
		In,
		Out
	}

	public class InOut : MonoBehaviour
#if UNITY_EDITOR
		, IEditModePlayback
#endif
	{
		[SerializeField]
		[HideInInspector]
		private float position;

		[SerializeField]
		[HideInInspector]
		private Direction direction;

		public Direction Direction
		{
			get
			{
				return direction;
			}
		}

		public float Position
		{
			get
			{
				return position;
			}

			set
			{
				onTween.Invoke( curve.Evaluate( position = Mathf.Clamp01( value ), reverse, invert ) );
			}
		}

		public float TimeElapsed
		{
			get
			{
				switch ( direction )
				{
				case Direction.In:
					return position * duration;
				case Direction.Out:
					return (1.0f - position) * duration;
				}

				return 0.0f;
			}
		}

		public float TimeRemaining
		{
			get
			{
				switch ( direction )
				{
				case Direction.In:
					return (1.0f - position) * duration;
				case Direction.Out:
					return position * duration;
				}

				return 0.0f;
			}
		}

		[SerializeField]
		[Range( 0.0f, 1.0f )]
		[Tooltip( "Set in Awake." )]
		private float initialPosition;

#if UNITY_EDITOR
		[SerializeField]
		[Range( 0.0f, 1.0f )]
		[Tooltip( "Used in edit mode." )]
		public float previewPosition = 1.0f;

		[SerializeField]
		[HideInInspector]
		private Direction previewDirection;

		bool IEditModePlayback.RequiresEditModeRepaint
		{
			get
			{
				return previewDirection != Direction.None;
			}
		}
#endif

		[SerializeField]
		[Tooltip( "How long it takes to tween from fully in to fully out or viceversa." )]
		private float duration = 1.0f;

		public float Duration
		{
			get
			{
				return duration;
			}
		}

		[SerializeField]
		private bool runInFixedUpdate;

		[SerializeField]
		private bool runInUnscaledTime;

		[Space]

		[SerializeField]
		private AnimationCurve curve = AnimationCurve.Linear( 0.0f, 0.0f, 1.0f, 1.0f );

		[SerializeField]
		private bool reverse, invert;

		[SerializeField]
		private TweenEvent onTween;

#if UNITY_EDITOR
		protected virtual void OnValidate ()
		{
			// enforce a curve that starts at 0,0 and ends at 1,1
			curve.NormalizeDomain().NormalizeRange();

			onTween.OnValidate();

			if ( !Application.isPlaying )
			{
				onTween.Invoke( curve.Evaluate( previewPosition, reverse, invert ) );
			}
		}

		bool IEditModePlayback.EditModeUpdate ()
		{
			bool done = false;

			switch ( previewDirection )
			{
			case Direction.In:
				previewPosition = Mathf.Clamp01( previewPosition + EditModePlayback.deltaTime / duration);

				done = previewPosition == 1.0f;
				break;

			case Direction.Out:
				previewPosition = Mathf.Clamp01( previewPosition - EditModePlayback.deltaTime / duration);

				done = previewPosition == 0.0f;
				break;

			default:
				return true;
			}

			onTween.Invoke( curve.Evaluate( previewPosition, reverse, invert ) );

			if ( done )
			{
				previewDirection = Direction.None;
			}

			return done;
		}
#endif

		protected virtual void Awake ()
		{
			Position = initialPosition;
		}

		protected virtual void FixedUpdate ()
		{
			if ( runInFixedUpdate )
			{
				Update( runInUnscaledTime ? Time.fixedUnscaledDeltaTime : Time.fixedDeltaTime );
			}
		}

		protected virtual void Update ()
		{
			if ( runInFixedUpdate )
			{
				return;
			}

			Update( runInUnscaledTime ? Time.unscaledDeltaTime : Time.deltaTime );
		}

		private void Update ( float deltaTime )
		{
			switch ( direction )
			{
			case Direction.In:
				Position += deltaTime / duration;

				if ( Position == 1.0f )
				{
					direction = Direction.None;
				}
				break;
			case Direction.Out:
				Position -= deltaTime / duration;

				if ( Position == 1.0f )
				{
					direction = Direction.None;
				}
				break;
			}
		}

		public void In ()
		{
			direction = Direction.In;
		}

		public void Out ()
		{
			direction = Direction.Out;
		}

		public void Stop ()
		{
			direction = Direction.None;
		}
	}
}
