using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;

[ExecuteInEditMode]
[RequireComponent( typeof( RectTransform ) )]
public sealed class CanvasSpring : MonoBehaviour
#if UNITY_EDITOR
		, IEditModePlayback
#endif
{
	public enum Solver
	{
		/// <summary>
		/// Cheap/fast, unstable.
		/// </summary>
		[Tooltip( "Cheap/fast, unstable." )]
		Explicit,

		/// <summary>
		/// Moderate performanceand/stability.
		/// </summary>
		[Tooltip( "Moderate performanceand/stability." )]
		Mixed,

		/// <summary>
		/// Expensive/slow, stable.
		/// </summary>
		[Tooltip( "Expensive/slow, stable." )]
		Implicit
	}

	public enum Axis
	{
		None,
		X,
		Y
	}

	public enum Mode
	{
		Auto,
		Anchor,
		Follow
	}

	private DrivenRectTransformTracker tracker;

	[SerializeField]
	private Axis axis;

	[SerializeField]
	private Mode mode;

	[SerializeField]
	private float anchor;

	[SerializeField]
	private RectTransform follow;

	[SerializeField]
	//[HideInInspector]
	private Vector2 autoPosition;
		
	[SerializeField]
	//[HideInInspector]
	private Vector3 realPosition;

	private bool realIsReady;

	[Header( "Mass Spring System" )]

	[SerializeField]
	private float mass = 0.1f;

	[SerializeField]
	private float strength = 16.0f;

	[SerializeField]
	private float damping = 0.8f;

	[Space]

	[SerializeField]
	private float sleepDistance = 1.0f;

	[SerializeField]
	private float sleepVelocity = 0.01f;

	[SerializeField]
	[HideInInspector]
	private float velocity;

	[Header( "Solver" )]

	[SerializeField]
	private Solver implementation = Solver.Explicit;

	[SerializeField]
	[Range( 1, 16 )]
	private int steps = 4;

	[SerializeField]
	[HideInInspector]
	private RectTransform rect;

#if UNITY_EDITOR
	[SerializeField]
	private bool executeInEditMode = true;

	private Axis activeAxis;
	private Mode activeMode;
	private bool autoIsReady;

	bool IEditModePlayback.RequiresEditModeRepaint
	{
		get
		{
			return velocity != 0.0f;
		}
	}

	private void OnValidate ()
	{
		bool newAxis = activeAxis != axis;
		bool newMode = activeMode != mode;

		Axis oldAxis = activeAxis;
		Mode oldMode = activeMode;

		if ( newAxis )
		{
			if ( activeAxis == Axis.None )
			{
				switch ( axis )
				{
				case Axis.X:
					anchor = rect.anchoredPosition.x;
					break;

				case Axis.Y:
					anchor = rect.anchoredPosition.y;
					break;

				default:
					tracker.Clear();
					velocity = 0.0f;
					break;
				}

				SetPosition( anchor );
			}

			activeAxis = axis;

			if ( activeAxis == Axis.None )
			{
				SetPosition( anchor );
				tracker.Clear();
				velocity = 0.0f;
			}
		}

		if ( axis != Axis.None && executeInEditMode )
		{
			EditModePlayback.Register( this );
		}
		else
		{
			EditModePlayback.Unregister( this );
		}

		if ( !Application.isPlaying && (newMode || newAxis) )
		{
			activeMode = mode;

			if ( mode == Mode.Auto && axis != Axis.None )
			{
				autoPosition = rect.position;

				autoIsReady = true;
			}
			else
			{
				autoIsReady = false;
			}
		}
	}

	private void Reset ()
	{
		rect = GetComponent<RectTransform>();
	}

	bool IEditModePlayback.EditModeUpdate ()
	{
		Step( EditModePlayback.deltaTime, steps );

		return axis == Axis.None;
	}

	private void OnEnable ()
	{
		SetPosition( GetPosition() );
	}
#endif

	private void Awake ()
	{
#if UNITY_EDITOR
		if ( !Application.isPlaying )
		{
			return;
		}
#endif

		if ( rect == null )
		{
			rect = GetComponent<RectTransform>();
		}

		autoPosition = rect.position;

		SetPosition( GetPosition() );
	}

	private void FixedUpdate ()
	{
#if UNITY_EDITOR
		if ( !Application.isPlaying )
		{
			return;
		}
#endif

		Step( Time.fixedDeltaTime, steps );
	}

	private void OnWillRenderObject ()
	{
#if UNITY_EDITOR
		if ( !Application.isPlaying && !executeInEditMode )
		{
			return;
		}
#endif

		Debug.Log( "OnWillRenderObject" );

		if ( mode == Mode.Auto && axis != Axis.None )
		{
			realPosition = rect.position;

			rect.position = new Vector3( autoPosition.x, autoPosition.y, realPosition.z );

			realIsReady = true;
		}
	}

	private void OnRenderObject ()
	{
#if UNITY_EDITOR
		if ( !Application.isPlaying && !executeInEditMode )
		{
			return;
		}
#endif

		if ( !realIsReady )
		{
			return;
		}

		if ( mode == Mode.Auto && axis != Axis.None )
		{
			rect.position = realPosition;

			realIsReady = false;
		}
	}

	private float GetTarget ()
	{
		switch ( mode )
		{
		case Mode.Auto:

			switch ( axis )
			{
			case Axis.X:
				return rect.position.x;
			case Axis.Y:
				return rect.position.y;
			}

			break;

		case Mode.Anchor:
			return anchor;

		case Mode.Follow:
			if ( follow == null )
			{
				Debug.LogError( "Follow transform cannot be null.", this );

				return GetPosition();
			}

			switch ( axis )
			{
			case Axis.X:
				return follow.position.x;
			case Axis.Y:
				return follow.position.y;
			}

			break;
		}

		throw new System.Exception();
	}

	private void Step ( float deltaTime, int steps )
	{
		if ( axis == Axis.None )
		{
			return;
		}
		
		if ( mass == 0.0f )
		{
			Debug.LogError( "CanvasSpring mass cannot be zero.", this );

			return;
		}

#if UNITY_EDITOR
		if ( !Application.isPlaying &&  mode == Mode.Auto && !autoIsReady )
		{
			autoPosition = rect.position;

			autoIsReady = true;
		}
#endif

		float target = GetTarget();
		float position = GetPosition();

		float delta = target - position;

		if ( velocity < sleepVelocity && Mathf.Abs( delta ) < sleepDistance )
		{
			return;
		}

		deltaTime /= steps;

		for ( int i = 0; i < steps; i++ )
		{
			position += velocity * Time.fixedDeltaTime;

			delta = target - position;

			switch ( implementation )
			{
			case Solver.Explicit:
				float springForce = delta * strength;
				float dampingForce = -velocity * damping;

				velocity += (springForce + dampingForce) * Time.fixedDeltaTime / mass;

				break;

			case Solver.Mixed:
				throw new System.NotImplementedException();

			case Solver.Implicit:
				throw new System.NotImplementedException();
			}

			if ( Mathf.Abs( delta ) < sleepDistance && Mathf.Abs( velocity ) < sleepVelocity )
			{
				position = anchor;
				velocity = 0.0f;
				break;
			}
		}

		SetPosition( position );
	}

	private bool TrySleep ( float distance )
	{
		if ( distance < sleepDistance && Mathf.Abs( velocity ) < sleepVelocity )
		{
			velocity = 0.0f;

			return true;
		}

		return false;
	}

	private float GetPosition ()
	{
		switch ( axis )
		{
		case Axis.X:
			return GetPosition2().x;

		case Axis.Y:
			return GetPosition2().y;
		}

		return float.NaN;
	}

	private void SetPosition ( float position )
	{
		tracker.Clear();

		switch ( axis )
		{
		case Axis.X:
			tracker.Add( this, rect, DrivenTransformProperties.AnchoredPositionX );
			SetPosition2( new Vector2( position, GetPosition2().y ) );
			break;

		case Axis.Y:
			tracker.Add( this, rect, DrivenTransformProperties.AnchoredPositionY );
			SetPosition2( new Vector2( GetPosition2().x, position ) );
			break;
		}
#if UNITY_EDITOR
		if ( axis != Axis.None )
		{
			UnityEditor.EditorUtility.SetDirty( rect );
		}
#endif
	}

	private Vector2 GetPosition2 ()
	{
		switch ( mode )
		{
		case Mode.Auto:
			return autoPosition;

		case Mode.Anchor:
			return rect.anchoredPosition;

		case Mode.Follow:
			return rect.position;
		}

		throw new System.Exception();
	}

	private void SetPosition2 ( Vector2 position )
	{
		switch ( mode )
		{
		case Mode.Auto:
			autoPosition = position;
			return;

		case Mode.Anchor:
			rect.anchoredPosition = position;
			return;

		case Mode.Follow:
			rect.position = position;
			return;
		}

		throw new System.Exception();
	}

	private void OnDisable ()
	{
		tracker.Clear();

		velocity = 0.0f;
	}
}
