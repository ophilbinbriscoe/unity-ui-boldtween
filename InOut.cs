using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace BoldTween
{
	public class InOut : MonoBehaviour
#if UNITY_EDITOR
		, IEditModePlayback
#endif
	{
		public enum Direction
		{
			None,
			In,
			Out
		}

		[SerializeField]
		[HideInInspector]
		private float position;

		[SerializeField]
		[HideInInspector]
		private Direction direction;

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

		[SerializeField]
		private bool runInFixedUpdate;

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
				switch ( direction )
				{
				case Direction.In:
					Position += Time.fixedDeltaTime / duration;

					if ( Position == 1.0f )
					{
						direction = Direction.None;
					}
					break;
				case Direction.Out:
					Position -= Time.fixedDeltaTime / duration;

					if ( Position == 1.0f )
					{
						direction = Direction.None;
					}
					break;
				}
			}
		}

		protected virtual void Update ()
		{
			if ( runInFixedUpdate )
			{
				return;
			}

			switch ( direction )
			{
			case Direction.In:
				Position += Time.deltaTime / duration;

				if ( Position == 1.0f )
				{
					direction = Direction.None;
				}
				break;
			case Direction.Out:
				Position -= Time.deltaTime / duration;

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
