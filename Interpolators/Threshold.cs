using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

namespace BoldTween
{
	[Serializable]
	public class BoolEvent : UnityEvent<bool> { }

	public class Threshold : Interpolator
	{
		[SerializeField]
		[Range( 0.0f, 1.0f )]
		private float falseThreshold = 0.0f;

		public float FalseThreshold
		{
			get
			{
				return falseThreshold;
			}

			set
			{
				falseThreshold = Mathf.Clamp01( value );

				Fix();
			}
		}

		[SerializeField]
		[Range( 0.0f, 1.0f )]
		private float trueThreshold = 1.0f;

		public float TrueThreshold
		{
			get
			{
				return trueThreshold;
			}

			set
			{
				trueThreshold = Mathf.Clamp01( value );

				Fix();
			}
		}

		private void Fix ()
		{
			if ( falseThreshold >= trueThreshold )
			{
				if ( trueThreshold == 0.0f )
				{
					trueThreshold += float.Epsilon;
				}
				else
				{
					falseThreshold -= float.Epsilon;
				}
			}
		}

#if UNITY_EDITOR
		protected override void OnValidate ()
		{
			base.OnValidate();

			Fix();
		}
#endif

		[SerializeField]
		private BoolEvent onValueChanged;

		[SerializeField]
		//[HideInInspector]
		private bool value;

		[SerializeField]
		//[HideInInspector]
		private bool init;

		private void Awake ()
		{
			init = false;
		}

		protected override void OnInterpolate ( float t )
		{
			bool value = this.value;

			if ( value )
			{
				if ( t <= falseThreshold )
				{
					value = false;
				}
			}
			else
			{
				if ( t >= trueThreshold )
				{
					value = true;
				}
			}

			if ( value != this.value || !init )
			{
#if UNITY_EDITOR
				init = Application.isPlaying;
#else
				init = true;
#endif

				onValueChanged.Invoke( this.value = value );
			}
		}
	}
}
