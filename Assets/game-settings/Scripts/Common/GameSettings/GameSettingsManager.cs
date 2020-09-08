using UnityEngine;
using System.Collections.Generic;

namespace Game
{
	public static class GameSettingsManager
	{
		private static readonly Vector2 MinScreenSize = new Vector2(1024, 768);

		// ------------------------------------------------------------------------------------------------------------
		#region main

		/// <summary> Restore saved settings. You normally call this as soon as the game has started </summary>
		public static void RestoreSettings()
		{
			// make sure it is initlaized
			InitializeVolumeTypes();

			// restore sound volume
			for (int i = 0; i < soundVolumes.Length; i++)
			{
				soundVolumes[i] = PlayerPrefs.GetFloat($"Settings.Volume.{i}", 1f);
				SetSoundVolume((SoundVolumeType)i, soundVolumes[i]);
			}

			// do not "restore" the screen resolution here
			// Unity will do so itself

			// restore quality setting
			int q = PlayerPrefs.GetInt("Settings.Quality", QualitySettings.GetQualityLevel());
			QualitySettings.SetQualityLevel(q);
		}

		#endregion
		// ------------------------------------------------------------------------------------------------------------
		#region resolution

		public static string[] ScreenModes = new[] { "Exclusive FullScreen", "FullScreen Window", "Maximized Window", "Windowed" };

		// for handlers interested in knowing when resolution changed
		public static event System.Action ResolutionChanged;
		private static List<Resolution> Resolutions = new List<Resolution>();

		/// <summary> A list of strings representing the available screen resolutions. </summary>
		public static List<string> ScreenResolutions
		{
			get
			{
				List<string> l = new List<string>(Screen.resolutions.Length);
				Resolutions.Clear();

				if (Screen.fullScreenMode == FullScreenMode.ExclusiveFullScreen)
				{
					// list all resolutionswith refresh rates  when in exclusive fullscreen
					for (int i = 0; i < Screen.resolutions.Length; i++)
					{
						if (Screen.resolutions[i].width >= MinScreenSize.x && Screen.resolutions[i].height >= MinScreenSize.y)
						{
							Resolutions.Add(Screen.resolutions[i]);
							l.Add(Screen.resolutions[i].ToString());
						}
					}
				}
				else
				{
					// dop not include refresh rate options when not exclusive
					for (int i = 0; i < Screen.resolutions.Length; i++)
					{
						if (Screen.resolutions[i].width >= MinScreenSize.x && Screen.resolutions[i].height >= MinScreenSize.y)
						{
							var res = $"{Screen.resolutions[i].width} x {Screen.resolutions[i].height}";
							if (!l.Contains(res))
							{
								Resolutions.Add(Screen.resolutions[i]);
								l.Add(res);
							}
						}
					}
				}

				return l;
			}
		}

		/// <summary> Get/Set current screen resolution index. This is an integer value representing a resolution from
		/// the list of supported resolutions. The list of resolutions is retrieved via ScreenResolutions property. 
		/// It will return -1 if the resolution index could not be determined </summary>
		public static int ScreenResolutionIndex
		{
			get
			{
				int _screenResolutionIdx = -1;

				int w = Screen.width;
				int h = Screen.height;
				if (Screen.fullScreenMode != FullScreenMode.Windowed)
				{
					w = Screen.currentResolution.width;
					h = Screen.currentResolution.height;
				}

				for (int i = Resolutions.Count - 1; i>= 0; i--)
				{
					if (w == Resolutions[i].width && h == Resolutions[i].height)
					{
						_screenResolutionIdx = i;
						if (Screen.fullScreenMode != FullScreenMode.ExclusiveFullScreen || 
							(	// the refresh can be out with about 1 or 2 values so just get it close
								Screen.currentResolution.refreshRate < Resolutions[i].refreshRate + 2 &&
								Screen.currentResolution.refreshRate > Resolutions[i].refreshRate - 2
							))
						{
							break; // break now if the exact resolution with refresh rate was found
						}
					}
				}

				return _screenResolutionIdx;
			}

			set
			{
				if (value >= 0 && value < Resolutions.Count)
				{
					Screen.SetResolution(Resolutions[value].width, Resolutions[value].height, Screen.fullScreenMode, Resolutions[value].refreshRate);
					PlayerPrefs.Save();
					
					ResolutionChanged?.Invoke();
				}
			}
		}

		/// <summary> Get or Set whether the game is in fullscreen mode or not. 
		/// This toggles between FullScreenWindow and Windowed. </summary>
		public static bool Fullscreen
		{
			get => Screen.fullScreenMode != FullScreenMode.Windowed;
			set => SetScreenMode(value ? FullScreenMode.FullScreenWindow : FullScreenMode.Windowed);
		}

		public static FullScreenMode ScreenMode
		{
			get => Screen.fullScreenMode;
			set => SetScreenMode(value);
		}

		public static int ScreenModeIndex
		{
			get => (int)Screen.fullScreenMode;
			set => SetScreenMode((FullScreenMode)value);
		}

		private static void SetScreenMode(FullScreenMode mode)
		{
			int w = Screen.width;
			int h = Screen.height;
			if (Screen.fullScreenMode != FullScreenMode.Windowed)
			{
				w = Screen.currentResolution.width;
				h = Screen.currentResolution.height;
			}

 			Screen.SetResolution(w, h, mode, Screen.currentResolution.refreshRate);
			PlayerPrefs.Save();

			ResolutionChanged?.Invoke();
		}

		#endregion
		// ------------------------------------------------------------------------------------------------------------
		#region quality

		/// <summary> An array of quality level names as set up in Quality Settings editor; menu: Edit > Project Settings > Quality </summary>
		public static string[] GFXQualityLevels => QualitySettings.names;

		/// <summary> Get or Set the quality level to use by the index into the list of defined quality levels. The 1st defined level's index will be 0, the 2nd will be 1, the 3rd will be 2, and so on.
		/// These quality levels are created in the Quality Settings editor; menu: Edit > Project Settings > Quality </summary>
		public static int GFXQualityLevelIndex
		{
			get => QualitySettings.GetQualityLevel();
			set
			{
				QualitySettings.SetQualityLevel(value);
				PlayerPrefs.SetInt("Settings.Quality", value);
				PlayerPrefs.Save();
			}
		}

		#endregion
		// ------------------------------------------------------------------------------------------------------------
		#region sound

		// same number of entries as SoundVolumeType enum
		private static float[] soundVolumes = null;
		private static List<SoundVolumeUpdater>[] soundVolumeUpdaters = null;

		// for handlers interested in knowing when any of the volume types changes
		public static event System.Action<SoundVolumeType, float> SoundVolumeChanged;

		public static void RegisterVolumeUpdater(SoundVolumeType type, SoundVolumeUpdater target)
		{
			InitializeVolumeTypes();

			int idx = (int)type;
			if (!soundVolumeUpdaters[idx].Contains(target))
			{
				soundVolumeUpdaters[idx].Add(target);
				target.UpdateVolume(soundVolumes[idx]);
			}
		}

		public static void RemoveVolumeUpdater(SoundVolumeType type, SoundVolumeUpdater target)
		{
			InitializeVolumeTypes();

			soundVolumeUpdaters[(int)type].Remove(target);
		}

		/// <summary> Set main sound volume. The value is a float value between 0 (no sound) and 1 (full). So (0.5) is half the sound volume.</summary>
		public static void SetMainSoundVolume(float value)
		{
			AudioListener.volume = Mathf.Clamp01(value);
		}

		/// <summary> Get main sound volume. The value is a float value between 0 (no sound) and 1 (full). So (0.5) is half the sound volume.</summary>
		public static float GetMainSoundVolume()
		{
			return AudioListener.volume;
		}

		/// <summary> Set sound volume of specified sound type. The value is a float value between 0 (no sound) and 1 (full). So (0.5) is half the sound volume.</summary>
		public static void SetSoundVolume(SoundVolumeType type, float value)
		{
			InitializeVolumeTypes();

			int idx = (int)type;
			value = Mathf.Clamp01(value);
			PlayerPrefs.SetFloat($"Settings.Volume.{idx}", value);
			PlayerPrefs.Save();

			if (type == SoundVolumeType.Main)
			{
				AudioListener.volume = value;
			}

			soundVolumes[idx] = value;
			for (int i = 0; i < soundVolumeUpdaters[idx].Count; i++)
			{
				soundVolumeUpdaters[idx][i].UpdateVolume(soundVolumes[idx]);
			}

			SoundVolumeChanged?.Invoke(type, value);
		}

		/// <summary> Get sound volume of specified sound type. The value is a float value between 0 (no sound) and 1 (full). So (0.5) is half the sound volume.</summary>
		public static float GetSoundVolume(SoundVolumeType type)
		{
			InitializeVolumeTypes();

			if (type == SoundVolumeType.Main)
			{
				return AudioListener.volume;
			}

			return soundVolumes[(int)type];
		}

		private static void InitializeVolumeTypes()
		{
			if (soundVolumes != null && soundVolumes.Length > 0)
			{
				return;
			}

			var count = System.Enum.GetNames(typeof(SoundVolumeType)).Length;
			soundVolumes = new float[count];
			soundVolumeUpdaters = new List<SoundVolumeUpdater>[count];

			for (int i = 0; i < count; i++)
			{
				soundVolumes[i] = 1f;
				soundVolumeUpdaters[i] = new List<SoundVolumeUpdater>(1);
			}
		}

		#endregion
		// ------------------------------------------------------------------------------------------------------------
	}
}
