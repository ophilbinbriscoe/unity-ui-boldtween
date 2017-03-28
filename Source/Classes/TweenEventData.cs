using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine.EventSystems;

namespace ToBoldlyPlay.Tweening
{
	public class TweenEventData : BaseEventData
	{
		public readonly float t;

		public TweenEventData ( EventSystem es, float t ) : base( es )
		{
			this.t = t;
		}
	}
}
