using System.Collections;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace BoldTween.Sequences
{
	public abstract class SequenceElement : ScriptableObject
	{
#if UNITY_EDITOR
		[SerializeField]
		[HideInInspector]
		private bool @break;

		public bool Break
		{
			get
			{
				return @break;
			}
		}
#endif

		public abstract void Reset ();

		public abstract float Tick ( float deltaTime );

		public abstract bool IsDone { get; }

#if UNITY_EDITOR
		public virtual void OnValidate () { }
#endif
	}
}
