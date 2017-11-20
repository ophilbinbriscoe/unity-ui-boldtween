using System.Collections;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.Events;

namespace BoldTween.Sequences
{
	public class Event : SequenceElement
	{
		[SerializeField]
		private UnityEvent onFire;

		[SerializeField]
		[HideInInspector]
		private bool fired;

		public override bool IsDone
		{
			get
			{
				return fired;
			}
		}

		public override void Reset ()
		{
			fired = false;
		}

		public override float Tick ( float deltaTime )
		{
			onFire.Invoke();

			fired = true;

			return deltaTime;
		}
	}
}
