using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace BoldTween
{
	/// <summary>
	/// 
	/// </summary>
	[Serializable]
	public struct Tween
	{
		public const float DEFAULT_EPSILON = 0.01f;

		[SerializeField]
		[HideInInspector]
		AnimationCurve curve;

		public AnimationCurve Curve
		{
			get
			{
				return curve;
			}
		}

		[SerializeField]
		[HideInInspector]
		private float duration, t, value;

		public float Duration
		{
			get
			{
				return duration;
			}
		}

		public float CurrentTime
		{
			get
			{
				return t;
			}
		}

		public float CurrentValue
		{
			get
			{
				return value;
			}
		}

		public bool HasFinished
		{
			get
			{
				return !Loop && (pong ? t == 0.0f : t == 1.0f);
			}
		}

		[SerializeField]
		[HideInInspector]
		private bool ping, pong;

		private TweenFlags flags;

		public TweenFlags Flags
		{
			get
			{
				return flags;
			}
		}

		public bool Min { get { return (flags & TweenFlags.Max) != 0; } }
		public bool Max { get { return (flags & TweenFlags.Max) != 0; } }
		public bool Reverse { get { return (flags & TweenFlags.Max) != 0; } }
		public bool Invert { get { return (flags & TweenFlags.Max) != 0; } }
		public bool Loop { get { return (flags & TweenFlags.Max) != 0; } }
		public bool PingPong { get { return (flags & TweenFlags.PingPong) != 0; } }
		public bool Unscaled { get { return (flags & TweenFlags.Max) != 0; } }

		public Tween ( AnimationCurve curve, float duration, TweenFlags flags, float value = default( float ), float epsilon = DEFAULT_EPSILON )
		{
			Assertions.Assert.IsTrue( curve.keys.Length > 1, "A Tween cannot be created using an AnimationCurve with less than 2 keys." );
			Assertions.Assert.AreEqual( curve.keys[0].time, 0.0f, "A Tween cannot be created using an AnimationCurve whose first key's time is anything other than zero." );
			Assertions.Assert.AreEqual( curve.keys[curve.keys.Length - 1].time, 1.0f, "A Tween cannot be created using an AnimationCurve whose last key's time is anything other than one." );

			this.curve = curve;
			this.duration = duration;
			this.flags = flags;
			this.value = value;

			t = 0.0f;
			ping = (flags & TweenFlags.PingPong) != 0;
			pong = false;

			Assertions.Assert.IsTrue( !(Min && Max), "Using the Min and Max flags at the same time is not supported." );
			Assertions.Assert.IsTrue( epsilon > 0.0f, "The parameter epsilon must be greater than zero." );

			if ( duration > 0.0f )
			{
				if ( Min )
				{
					float v = Evaluate( t );

					// Update Tween until v is lesser or equal to the current value
					while ( v > value )
					{
						t = Mathf.Clamp01( t + epsilon );

						v = Evaluate( t );

						if ( t == 1.0f )
						{
							break;
						}
					}
				}

				if ( Max )
				{
					float v = Evaluate( t );

					// Update Tween until v is greater or equal to the current value
					while ( v < value )
					{
						t = Mathf.Clamp01( t + epsilon );

						v = Evaluate( t );

						if ( t == 1.0f )
						{
							break;
						}
					}
				}
			}
		}

		public void FixedUpdate ()
		{
			if ( Unscaled )
			{
				Update( Time.fixedUnscaledDeltaTime );
			}
			else
			{
				Update( Time.fixedDeltaTime );
			}
		}

		public void Update ()
		{
			if ( Unscaled )
			{
				Update( Time.unscaledDeltaTime );
			}
			else
			{
				Update( Time.deltaTime );
			}
		}

		public void Update( float deltaTime )
		{
			Assertions.Assert.IsFalse( HasFinished, "Cannot update a Tween that has already finished." );

			deltaTime /= duration;

			float overflow;

			if ( pong )
			{
				overflow = deltaTime - t;

				t = Mathf.Clamp01( t - deltaTime );
			}
			else
			{
				overflow = deltaTime - (1.0f - t);

				t = Mathf.Clamp01( t + deltaTime );
			}

			if ( t == 1.0f )
			{
				if ( ping )
				{
					t -= overflow;

					ping = false;
					pong = true;
				}
				else if ( Loop )
				{
					t = overflow;
				}
			}
			else if ( t == 0.0f && pong & Loop )
			{
				t = overflow;

				ping = true;
				pong = false;
			}
		}

		public float Evaluate ( float t )
		{
			Assertions.Assert.IsTrue( t <= 1.0f && t >= 0.0f, "The parameter t must be in the range [0, 1]." );

			if ( Reverse )
			{
				t = 1.0f - t;
			}

			t = curve.Evaluate( t );

			if ( Invert )
			{
				t = 1.0f - t;
			}

			if ( Min )
			{
				t = Mathf.Min( value, t );
			}

			if ( Max )
			{
				t = Mathf.Max( value, t );
			}

			return t;
		}
	}
}
