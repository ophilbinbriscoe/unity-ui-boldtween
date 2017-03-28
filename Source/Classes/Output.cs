using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace ToBoldlyPlay.Tweening
{
	public partial class Tweener
	{
		[Serializable]
		public class Output
		{
			[EnumField( EnumStyle.Mask )]
			public ModifierFlags modifiers;

			public float multiplier;

			public UnityEvent onStart, onUpdate, onFinish;
		}
	}
}
