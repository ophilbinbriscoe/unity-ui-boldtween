using System.Collections;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using System;

namespace BoldTween.Sequences
{
	public class WaitForKeyUp : Yield
	{
		[SerializeField]
		private KeyCode keyCode = KeyCode.Space;

		protected override bool ContinueWaiting ()
		{
			return !Input.GetKeyUp( keyCode );
		}
	}
}
