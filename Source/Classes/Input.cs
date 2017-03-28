using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

using UnityEngine;

namespace ToBoldlyPlay.Tweening
{
	public partial class Tweener
	{
		[Serializable]
		public class Input
		{
			[SerializeField]
			private EventObject trigger;

			public EventObject Trigger
			{
				get
				{
					return trigger;
				}
			}

			[ListField]
			[SerializeField]
			private Output mapping;

			public Output Mapping
			{
				get
				{
					return mapping;
				}
			}
		}
	}
}
