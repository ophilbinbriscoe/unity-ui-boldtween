using System.Collections;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using System;

namespace BoldTween.Sequences
{
	public class WaitUntilNextFrame : Yield
	{
		[SerializeField]
		[HideInInspector]
		private bool done;

		protected override bool ContinueWaiting ()
		{
			return done & (done = true);
		}

		public override void Reset ()
		{
			base.Reset();

			done = false;
		}
	}
}
