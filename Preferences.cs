using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;

namespace BoldTween
{

	public static class Preferences
	{
		public interface IPreference
		{
			void RestoreDefault ();
		}

		public interface IPreference<T> : IPreference
		{
			event UnityAction<T> OnSetValue;
			void Set ( T value );
			T Get ();
			T GetDefault ();
		}

		#region Property type definitions
		private abstract class EditorPref<T> : IPreference<T>
		{
			public readonly string key;

			private T value;
			private T defaultValue;

			public event UnityAction<T> OnSetValue;

			public void Set ( T value )
			{
				SetEditorPref( this.value = value );

				if ( OnSetValue != null )
				{
					OnSetValue( value );
				}
			}

			public T Get ()
			{
				return value;
			}

			public T GetDefault ()
			{
				return defaultValue;
			}

			public void RestoreDefault ()
			{
				EditorPrefs.DeleteKey( key );
			}

			protected EditorPref ( string key, T defaultValue )
			{
				this.key = key;
				this.defaultValue = defaultValue;

				value = GetEditorPref( defaultValue );
			}

			protected abstract void SetEditorPref ( T value );
			protected abstract T GetEditorPref ( T defaultValue );
		}

		private class EditorPrefInt : EditorPref<int>
		{
			public EditorPrefInt ( string key, int defaultValue ) : base( key, defaultValue ) { }

			protected override int GetEditorPref ( int defaultValue )
			{
				return EditorPrefs.GetInt( key, defaultValue );
			}

			protected override void SetEditorPref ( int value )
			{
				EditorPrefs.SetInt( key, value );
			}
		}

		private class EditorPrefEnum<T> : EditorPref<T> where T : struct
		{
			public EditorPrefEnum ( string key, T defaultValue ) : base( key, defaultValue )
			{
				if ( !typeof( T ).IsEnum )
				{
					throw new Exception( "Generic type parameter for a EditorPrefEnum must be an enum type." );
				}
			}

			protected override T GetEditorPref ( T defaultValue )
			{
				return (T) (object) EditorPrefs.GetInt( key, Convert.ToInt32( defaultValue ) );
			}

			protected override void SetEditorPref ( T value )
			{
				EditorPrefs.SetInt( key, Convert.ToInt32( value ) );
			}
		}

		private class EditorPrefsFloat : EditorPref<float>
		{
			public EditorPrefsFloat ( string key, float defaultValue ) : base( key, defaultValue ) { }

			protected override float GetEditorPref ( float defaultValue )
			{
				return EditorPrefs.GetFloat( key, defaultValue );
			}

			protected override void SetEditorPref ( float value )
			{
				EditorPrefs.SetFloat( key, value );
			}
		}

		private class EditorPrefsString : EditorPref<string>
		{
			public EditorPrefsString ( string key, string defaultValue ) : base( key, defaultValue ) { }

			protected override string GetEditorPref ( string defaultValue )
			{
				return EditorPrefs.GetString( key, defaultValue );
			}

			protected override void SetEditorPref ( string value )
			{
				EditorPrefs.SetString( key, value );
			}
		}
		#endregion

		public static readonly IPreference<float> fxVolume = new EditorPrefsFloat( "fxVolume", 0.75f );
		public static readonly IPreference<float> musicVolume = new EditorPrefsFloat( "musicVolume", 0.75f );

		public static void RestoreAllDefaults ()
		{
			foreach ( var field in typeof( Preferences ).GetFields( System.Reflection.BindingFlags.Static | System.Reflection.BindingFlags.Public ) )
			{
				(field.GetValue( null ) as IPreference).RestoreDefault();
			}
		}
	}
}
