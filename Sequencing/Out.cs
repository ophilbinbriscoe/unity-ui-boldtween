using System.Collections;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace BoldTween.Sequences
{
	public class Out : InOutElement
	{
		public override float Tick ( float deltaTime )
		{
			float overflow = base.Tick( deltaTime );

			tweener.Position = 1.0f - elapsed / GetDuration();

			return overflow;
		}
	}
}
