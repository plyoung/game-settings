using UnityEngine;

namespace Game
{
	[AddComponentMenu("GameSettings/Sound Volume Updater")]
	public class SoundVolumeUpdater : MonoBehaviour
	{
		[Tooltip("The AudioSource for which this updater will control the volume")]
		public AudioSource target;

		[Tooltip("The sound volume type this updater should bind to. It will set the target AudioSource to whatever changes takes place on the specified volume type only.")]
		public SoundVolumeType volumeType = SoundVolumeType.GUI;

		protected void Reset()
		{
			target = GetComponent<AudioSource>();
		}

		protected void Awake()
		{
			if (target == null)
			{
				target = GetComponent<AudioSource>();
				if (target == null)
				{
					Debug.LogError("[SoundVolumeUpdater] Could not find any AudioSource on the object.", gameObject);
					return;
				}
			}

			GameSettingsManager.RegisterVolumeUpdater(volumeType, this);
		}

		private void OnDestroy()
		{
			GameSettingsManager.RemoveVolumeUpdater(volumeType, this);
		}

		public void UpdateVolume(float v)
		{
			if (target == null) return;
			target.volume = v;
		}

		// ------------------------------------------------------------------------------------------------------------
	}
}
