using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace Game
{
	[AddComponentMenu("GameSettings/UI/Quality Dropdown")]
	public class QualityDropdown : MonoBehaviour
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
					Debug.Log("[QualityDropdown] Could not find any TextMeshPro Dropdown component on the GameObject.", gameObject);
					return;
				}
			}

			List<TMP_Dropdown.OptionData> opts = new List<TMP_Dropdown.OptionData>();
			foreach (string s in GameSettingsManager.GFXQualityLevels)
			{
				opts.Add(new TMP_Dropdown.OptionData(s));
			}

			targetElement.ClearOptions();
			targetElement.AddOptions(opts);
		}

		private void Start()
		{
			if (targetElement != null)
			{
				targetElement.value = GameSettingsManager.GFXQualityLevelIndex;
				targetElement.onValueChanged.AddListener(OnValueChange);
			}
		}

		private void OnValueChange(int idx)
		{
			GameSettingsManager.GFXQualityLevelIndex = idx;
		}

		// ------------------------------------------------------------------------------------------------------------
	}

}
