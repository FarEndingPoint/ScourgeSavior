using UnityEngine;
using UnityEngine.UI;
using QFramework;
using UnityEngine.SceneManagement;

namespace ns
{
	public class SettingsPanelData : UIPanelData
	{
	}
	public partial class SettingsPanel : UIPanel
	{
		[SerializeField] Button level1, level2, level3, level4, level5, level6;
        [SerializeField] Scrollbar music, sound;
        [SerializeField] Text musicNum, soundNum;
		[SerializeField] Button MainMenu;
		GameData gameData;

        protected override void OnInit(IUIData uiData = null)
		{
			mData = uiData as SettingsPanelData ?? new SettingsPanelData();
            // please add init code here
        }
		
		protected override void OnOpen(IUIData uiData = null)
		{
            gameData = Main.Interface.GetModel<GameData>();
        }
		
		protected override void OnShow()
		{
            level1.interactable = gameData.isLevel1Unlocked;
			level2.interactable = gameData.isLevel2Unlocked;
			level3.interactable = gameData.isLevel3Unlocked;
			level4.interactable = gameData.isLevel4Unlocked;
			level5.interactable = gameData.isLevel5Unlocked;
			level6.interactable = gameData.isLevel6Unlocked;
			music.value = gameData.music / 100.0f;
            sound.value = gameData.sound / 100.0f;
            musicNum.text = gameData.music.ToString();
            soundNum.text = gameData.sound.ToString();
        }
		
		protected override void OnHide()
		{
        }
		
		protected override void OnClose()
		{
		}

		public void LoadLevel(int level)
		{
			AudioKit.PlaySound(Main.Interface.GetModel<GameData>().soundPath + "Button" + Random.Range(1, 4).ToString());
			if (LevelController.Instance.level == level) return;
            UIKit.ClosePanel<ReadyPanel>();
            CloseSelf();
            Time.timeScale = 1;
            SceneManager.LoadScene(level);
        }

		public void OnMusicValueChange()
		{
			AudioKit.Settings.MusicVolume.Value = music.value;
			gameData.music = (int)Mathf.Floor(music.value * 100);
            musicNum.text = Mathf.Floor(music.value * 100).ToString();
            PlayerPrefs.SetInt("music", gameData.music);
        }

        public void OnSoundValueChange()
        {
            AudioKit.Settings.SoundVolume.Value = sound.value;
            gameData.sound = (int)Mathf.Floor(sound.value * 100);
            soundNum.text = Mathf.Floor(sound.value * 100).ToString();
            PlayerPrefs.SetInt("sound", gameData.sound);
        }

        public void LoadMainMenu()
		{
            AudioKit.PlaySound(Main.Interface.GetModel<GameData>().soundPath + "Button" + Random.Range(1, 4).ToString());
            UIKit.ClosePanel<ReadyPanel>();
            CloseSelf();
            Time.timeScale = 1;
            SceneManager.LoadScene(0);
        }
    }
}
