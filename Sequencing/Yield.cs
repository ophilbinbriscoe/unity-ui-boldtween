using System.Collections;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace BoldTween.Sequences
{
	public abstract class Yield : SequenceElement
	{
		[SerializeField]
		private bool isDone;

		public override bool IsDone
		{
			get
			{
				return isDone;
			}
		}

		public override void Reset ()
		{
			isDone = false;
		}

		public override float Tick ( float deltaTime )
		{
			if ( ContinueWaiting() )
			{
				return 0.0f;
			}
			else
			{
				isDone = true;

				return deltaTime;
			}
		}

		protected abstract bool ContinueWaiting ();
	}
}
