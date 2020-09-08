using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace Game
{
	[AddComponentMenu("GameSettings/UI/Resolution Dropdown")]
	public class ResolutionDropdown : MonoBehaviour
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
					Debug.Log("[ResolutionDropdown] Could not find any TextMeshPro Dropdown component on the GameObject.", gameObject);
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
			// resolution switch only happens at end of current frame. switching from/to exclusive mode can 
			// have a longer delay before correct values can be read. just waiting one WaitForFixedUpdate in 
			// that case is not enough so I'm using about a seconds here. 
			// TODO: Is there better method?
			yield return new WaitForSeconds(0.85f);
			RefreshControl();
		}

		private void RefreshControl()
		{
			if (targetElement != null)
			{
				targetElement.ClearOptions();
				targetElement.onValueChanged.RemoveAllListeners();

				List<TMP_Dropdown.OptionData> opts = new List<TMP_Dropdown.OptionData>();
				foreach (string s in GameSettingsManager.ScreenResolutions)
				{
					opts.Add(new TMP_Dropdown.OptionData(s));
				}

				targetElement.AddOptions(opts);
				targetElement.value = GameSettingsManager.ScreenResolutionIndex;
				targetElement.onValueChanged.AddListener(OnValueChange);
			}
		}

		private void OnValueChange(int idx)
		{
			GameSettingsManager.ScreenResolutionIndex = idx;
		}

		// ------------------------------------------------------------------------------------------------------------
	}

}
