using System.Collections;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace BoldTween.Sequences
{
	public class Delay : SequenceElement
	{
		[SerializeField]
		protected float duration = 1.0f;

		[SerializeField]
		[HideInInspector]
		protected float elapsed;

		public override bool IsDone
		{
			get
			{
				return elapsed == duration;
			}
		}

		public override void Reset ()
		{
			elapsed = 0.0f;
		}

		public override float Tick ( float deltaTime )
		{
			float remaining = duration - elapsed;

			if ( remaining < deltaTime )
			{
				elapsed = duration;

				return deltaTime - remaining;
			}
			else
			{
				elapsed += deltaTime;

				return 0.0f;
			}
		}
	}
}
