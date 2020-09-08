using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace Game
{
	[AddComponentMenu("GameSettings/UI/Screen Mode Dropdown")]
	public class ScreenModeDropdown : MonoBehaviour
	{
		[SerializeField] private TMP_Dropdown targetElement;

		// ------------------------------------------------------------------------------------------------------------

		private void Reset()
		{
			targetElement = GetComponentInChildren<TMP_Dropdown>();
		}

		private void Awake()
		{
			if (targetElement == null)
			{
				targetElement = GetComponentInChildren<TMP_Dropdown>();
				if (targetElement == null)
				{
					Debug.Log("[ScreenModeDropdown] Could not find any TextMeshPro Dropdown component on the GameObject.", gameObject);
					return;
				}
			}
		}

		private void Start()
		{
			GameSettingsManager.ResolutionChanged += RefreshControlDelayed;
		}

		private void OnDestroy()
		{
			GameSettingsManager.ResolutionChanged -= RefreshControlDelayed;
		}

		private void OnEnable()
		{
			RefreshControl();
		}

		private void RefreshControlDelayed()
		{
			StartCoroutine(RefreshControlSequence());
		}

		private IEnumerator RefreshControlSequence()
		{
			// resolution switch happens at end of frame so need to wait before refreshing values
			yield return new WaitForFixedUpdate();
			RefreshControl();
		}

		private void RefreshControl()
		{
			if (targetElement != null)
			{
				targetElement.ClearOptions();
				targetElement.onValueChanged.RemoveAllListeners();

				List<TMP_Dropdown.OptionData> opts = new List<TMP_Dropdown.OptionData>();
				foreach (string s in GameSettingsManager.ScreenModes)
				{
					opts.Add(new TMP_Dropdown.OptionData(s));
				}

				targetElement.AddOptions(opts);
				targetElement.value = GameSettingsManager.ScreenModeIndex;
				targetElement.onValueChanged.AddListener(OnValueChange);
			}
		}

		private void OnValueChange(int idx)
		{
			GameSettingsManager.ScreenModeIndex = idx;
		}

		// ------------------------------------------------------------------------------------------------------------
	}

}
