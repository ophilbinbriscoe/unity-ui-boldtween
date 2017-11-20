using System.Collections;
using System.Collections.Generic;
using System.Reflection;

using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace BoldTween
{
	[RequireComponent( typeof( CanvasGroup ) )]
	public class AlphaInterpolator : Interpolator
	{
		[System.Serializable]
		private class AlphaEvent : UnityEvent<Color> { }

		[SerializeField]
		[Range( 0.0f, 1.0f )]
		private float a = 0.0f, b = 1.0f;

		public float A
		{
			get
			{
				return a;
			}

			set
			{
				a = Mathf.Clamp01( value );
			}
		}

		public float B
		{
			get
			{
				return b;
			}

			set
			{
				b = Mathf.Clamp01( value );
			}
		}

		[SerializeField]
		private AlphaEvent targets;

		protected override void OnInterpolate ( float t )
		{
			float alpha = a + (b - a) * t;

			int persistentEventCount = targets.GetPersistentEventCount();

			for (int i = 0; i < persistentEventCount; i++)
			{
				var target = targets.GetPersistentTarget( i );
				var name = targets.GetPersistentMethodName( i );

				if ( name.StartsWith( "set_" ) )
				{
					name = name.Substring( 4, name.Length - 4 );

					var field = target.GetType().GetField( name );

					if ( field != null )
					{
						if ( field.FieldType == typeof( Color ) )
						{
							var color = (Color) field.GetValue( target );

							color.a = alpha;

							field.SetValue( target, color );
						}
						else if ( field.FieldType == typeof( Color32 ) )
						{
							var color = (Color32) field.GetValue( target );

							color.a = (byte) Mathf.RoundToInt( 255 * alpha );

							field.SetValue( target, color );
						}
					}
					else
					{
						var property = target.GetType().GetProperty( name );

						if ( property != null )
						{
							if ( property.PropertyType == typeof( Color ) )
							{
								var color = (Color) property.GetValue( target, null );

								color.a = alpha;

								property.SetValue( target, color, null );
							}
							else if ( property.PropertyType == typeof( Color32 ) )
							{
								var color = (Color32) property.GetValue( target, null );

								color.a = (byte) Mathf.RoundToInt( 255 * alpha );

								property.SetValue( target, color, null );
							}
						}
					}
				}
			}
		}
	}
}
