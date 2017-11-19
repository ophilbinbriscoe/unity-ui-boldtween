using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.Events;

namespace BoldTween
{
	public interface IMultiObjectBasedInterpolator<O> where O : UnityEngine.Object
	{
		ReadOnlyCollection<O> Targets { get; }

		void Add ( O target );
		void Remove ( O target );
	}
}
