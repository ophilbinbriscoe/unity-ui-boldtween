using System.Collections;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using System;

namespace BoldTween.Sequences
{
	public class WaitForButtonUp : Yield
	{
		[SerializeField]
		private string buttonName = "Submit";

		protected override bool ContinueWaiting ()
		{
			return !Input.GetButtonUp( buttonName );
		}
	}
}
