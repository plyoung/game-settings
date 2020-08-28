using UnityEngine;
using UnityEngine.UI;

namespace Game
{
	[AddComponentMenu("GameSettings/UI/Fullscreen Toggle")]
	public class FullscreenToggle : MonoBehaviour
	{
		[SerializeField] private Toggle targetElement;

		// ------------------------------------------------------------------------------------------------------------

		private void Reset()
		{
			targetElement = GetComponentInChildren<Toggle>();
		}

		private void Start()
		{
			if (targetElement == null)
			{
				targetElement = GetComponentInChildren<Toggle>();
				if (targetElement == null)
				{
					Debug.Log("[FullscreenToggle] Could not find any Toggle component on the GameObject.", gameObject);
					return;
				}
			}

			targetElement.isOn = GameSettingsManager.Fullscreen;
			targetElement.onValueChanged.AddListener(OnValueChange);
		}

		private void OnValueChange(bool isOn)
		{
			GameSettingsManager.Fullscreen = isOn;
		}

		// ------------------------------------------------------------------------------------------------------------
	}

}
