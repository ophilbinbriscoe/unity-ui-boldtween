using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace BoldTween
{
	public enum AssertionBehaviour
	{
		AlwaysTrue,
		Supress,
		LogError,
		Exception
	}

	public static class Assertions
	{
		public static AssertionBehaviour behaviour;

		public static class Assert
		{
			public static bool IsTrue ( bool a, string message = "", Object context = null )
			{
				return AssertInternal( a, message, context );
			}

			public static bool IsFalse ( bool a, string message = "", Object context = null )
			{
				return AssertInternal( !a, message, context );
			}

			public static bool IsNotNull ( object o, string message = "", Object context = null )
			{
				return AssertInternal( o != null, message, context );
			}

			public static bool IsNull ( object o, string message = "", Object context = null )
			{
				return AssertInternal( o == null, message, context );
			}
			 
			public static bool AreEqual<T> ( T a, T b, string message = "", Object context = null )
			{
				return AssertInternal( (a == null && b == null) || (a.Equals( b )), message, context );
			}

			public static bool AreNotEqual<T> ( T a, T b, string message = "", Object context = null )
			{
				return AssertInternal( a == null ? b != null : !a.Equals( b ), message, context );
			}

			private static bool AssertInternal ( bool result, string message, Object context )
			{
				switch ( behaviour )
				{
				case AssertionBehaviour.AlwaysTrue:
					return true;

				case AssertionBehaviour.LogError:
					if ( !result )
					{
						if ( context == null )
						{
							Debug.LogError( message );
						}
						else
						{
							Debug.LogError( message, context );
						}
					}
					break;

				case AssertionBehaviour.Exception:
					throw new System.Exception( message );
				}

				return result;
			}
		}
	}
}
