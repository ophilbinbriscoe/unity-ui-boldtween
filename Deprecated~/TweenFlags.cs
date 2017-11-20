using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BoldTween
{
	[Flags]
	public enum TweenFlags : byte
	{
		None = 0,

		/// <summary>
		/// Tween will only start from a time such that the output is no greater than the starting value.
		/// This typically results in a shorter tween.
		/// </summary>
		Min = 1,

		/// <summary>
		/// Tween will only start from a time such that the output is no less than the starting value.
		/// This typically results in a shorter tween.
		/// </summary>
		Max = 2,

		/// <summary>
		/// The curve will be evaluated at one minus the current time.
		/// </summary>
		Reverse = 4,

		/// <summary>
		/// The output will be one minus the result of evaluating the curve.
		/// </summary>
		Invert = 8,

		/// <summary>
		/// Tween will only start from a time such that the output is no greater than the starting value, and the curve will be evaluated at one minus the current time.
		/// This typically results in a shorter tween.
		/// </summary>
		Minverse = Min | Reverse,

		/// <summary>
		/// Tween will only start from a time such that the output is no greater than the starting value, and the output will be one minus the result of evaluating the curve.
		/// This typically results in a shorter tween.
		/// </summary>
		Minvert = Min | Invert,

		/// <summary>
		/// Tween will play indefinitely.
		/// </summary>
		Loop = 16,

		/// <summary>
		/// Tween will play forward then backward.
		/// </summary>
		PingPong = 32,

		/// <summary>
		/// Tween will play forward then backward indefinitely.
		/// </summary>
		PingPongLoop = Loop | PingPong,

		/// <summary>
		/// Tween will run on unscaled time.
		/// </summary>
		Unscaled = 64
	}
}
