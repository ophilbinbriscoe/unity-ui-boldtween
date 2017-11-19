using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.Events;

namespace BoldTween
{
	public interface IObjectBasedInterpolator<O> where O : UnityEngine.Object
	{
		O Target { get; set; }
	}
}
