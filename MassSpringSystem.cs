﻿using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using System;

namespace BoldTween
{
	[ExecuteInEditMode]
	public sealed class MassSpringSystem : Interpolator
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

		[SerializeField]
		[Tooltip( "Current state of the mass spring system.")]
		private float currentPosition;

		[Header( "Mass Spring System" )]

		[SerializeField]
		private float mass = 0.1f;

		[SerializeField]
		private float strength = 10.0f;

		[SerializeField]
		private AnimationCurve dampingCurve = AnimationCurve.Linear( 0.0f, 0.9f, 1.0f, 0.9f );

		[SerializeField]
		private float dampingCoefficient = 1.0f;

		[Space]

		[SerializeField]
		private float sleepDistance = 0.001f;

		[SerializeField]
		private float sleepVelocity = 0.001f;

		[SerializeField]
		//[HideInInspector]
		private float velocity;

		[Header( "Solver" )]

		[SerializeField]
		private Solver implementation = Solver.Explicit;

		[SerializeField]
		[Range( 1, 16 )]
		[Tooltip( "How many solver iterations to perform each time FixedUpdate is called.")]
		private int steps = 4;

#if UNITY_EDITOR
		[SerializeField]
		private bool executeInEditMode = true;

		bool IEditModePlayback.RequiresEditModeRepaint
		{
			get
			{
				return velocity != 0.0f;
			}
		}

		[SerializeField]
		[HideInInspector]
		private int listenerCount;

		protected override void OnValidate ()
		{
			base.OnValidate();

			if ( executeInEditMode )
			{
				EditModePlayback.Register( this );
			}
			else
			{
				EditModePlayback.Unregister( this );
			}

			if ( listenerCount < (listenerCount = onPositionChanged.GetPersistentEventCount()) )
			{
				onPositionChanged.SetPersistentListenerState( listenerCount - 1, UnityEngine.Events.UnityEventCallState.EditorAndRuntime );
			}

			onPositionChanged.Invoke( currentPosition );	
		}

		bool IEditModePlayback.EditModeUpdate ()
		{
			Step( EditModePlayback.deltaTime, steps );

			return !executeInEditMode;
		}
#endif

		[Space]

		[SerializeField]
		private TweenEvent onPositionChanged;

		private void Start ()
		{
			onPositionChanged.Invoke( currentPosition = position );
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

		private void Step ( float deltaTime, int steps )
		{
			if ( mass == 0.0f )
			{
				Debug.LogError( "CanvasSpring mass cannot be zero.", this );

				return;
			}

			float distance = position - currentPosition;

			if ( CanSleepAt( distance ) )
			{
				return;
			}

			deltaTime /= steps;

			for ( int i = 0; i < steps; i++ )
			{
				currentPosition += velocity * deltaTime;

				distance = position - currentPosition;

				switch ( implementation )
				{
				case Solver.Explicit:
					float springForce = distance * strength;
					float dampingForce = -velocity * dampingCurve.Evaluate( velocity ) * dampingCoefficient;

					velocity += (springForce + dampingForce) * deltaTime / mass;

					break;

				case Solver.Mixed:
					throw new System.NotImplementedException();

				case Solver.Implicit:
					throw new System.NotImplementedException();
				}

				if ( CanSleepAt( distance ) )
				{
					currentPosition = position;
					velocity = 0.0f;

					break;
				}
			}

			onPositionChanged.Invoke( currentPosition );
		}

		private bool CanSleepAt ( float distance )
		{
			return Mathf.Abs( distance ) < sleepDistance && Mathf.Abs( velocity ) < sleepVelocity;
		}

		private void OnDisable ()
		{
			velocity = 0.0f;
		}

		protected override void OnInterpolate ( float value )
		{
			position = value;
		}
	}
}