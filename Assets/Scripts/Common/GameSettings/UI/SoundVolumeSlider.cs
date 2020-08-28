using UnityEngine;
using UnityEngine.UI;

namespace Game
{
	[AddComponentMenu("GameSettings/UI/Sound Volume Slider")]
	public class SoundVolumeSlider : MonoBehaviour
	{
		[SerializeField] private Slider targetElement;
		[SerializeField] private SoundVolumeType volumeType = SoundVolumeType.Main;

		// ------------------------------------------------------------------------------------------------------------

		private void Reset()
		{
			targetElement = GetComponentInChildren<Slider>();
		}

		private void Start()
		{
			if (targetElement == null)
			{
				targetElement = GetComponentInChildren<Slider>();
				if (targetElement == null)
				{
					Debug.Log("[SoundVolumeSlider] Could not find any Slider component on the GameObject.", gameObject);
					return;
				}
			}

			targetElement.value = GameSettingsManager.GetSoundVolume(volumeType);
			targetElement.onValueChanged.AddListener(OnValueChange);
		}

		private void OnValueChange(float value)
		{
			GameSettingsManager.SetSoundVolume(volumeType, value);
		}

		// ------------------------------------------------------------------------------------------------------------
	}

}
