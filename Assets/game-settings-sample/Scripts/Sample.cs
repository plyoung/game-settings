using TMPro;
using UnityEngine;

namespace Game
{
	public class Sample : MonoBehaviour
	{
		[SerializeField] private AudioSource testSound=null;
		[SerializeField] private TMP_Text resolutionLabel=null;

		private void Start()
		{
			// initialize and restore game settings as soon as game started
			GameSettingsManager.RestoreSettings();
		}

		private void LateUpdate()
		{
			resolutionLabel.text = $"{Screen.currentResolution} {Screen.fullScreenMode} ({Screen.width} x {Screen.height} {(Screen.fullScreen ? "Fullscreen" : "Window")})";
		}

		public void PlayTestSound()
		{
			testSound.Play();
		}

		public void QuitGame()
		{
			Application.Quit();
		}

		// ------------------------------------------------------------------------------------------------------------
	}
}
