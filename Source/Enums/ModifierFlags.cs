using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ToBoldlyPlay.Tweening
{
	[Flags]
	public enum ModifierFlags
	{
		Min = 1,
		Max = 2,
		Reverse = 4,
		Invert = 8,
		Debug = 16
	}
}