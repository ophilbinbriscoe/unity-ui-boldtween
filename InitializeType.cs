using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BoldTween
{
	[Flags]
	public enum InitializeType : byte
	{
		None,
		OnEnable,
		OnAwake,
		OnStart
	}
}
