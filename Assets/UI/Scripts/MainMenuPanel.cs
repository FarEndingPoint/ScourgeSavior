using UnityEngine;
using UnityEngine.UI;
using QFramework;
using UnityEngine.SceneManagement;

namespace ns
{
	public class MainMenuPanelData : UIPanelData
	{
	}
	public partial class MainMenuPanel : UIPanel
	{
        float backgroundY = 0;
        float backgroundY1 = 1080f;
        RectTransform rect;
        [SerializeField] Image tittle;
        bool isStarted = false;
		GameData gameData;

        protected override void OnInit(IUIData uiData = null)
		{
			mData = uiData as MainMenuPanelData ?? new MainMenuPanelData();
            // please add init code here
            rect = GetComponent<RectTransform>();
            gameData = Main.Interface.GetModel<GameData>();
        }
		
		protected override void OnOpen(IUIData uiData = null)
		{
		}
		
		protected override void OnShow()
		{
		}
		
		protected override void OnHide()
		{
		}
		
		protected override void OnClose()
		{
		}

        public void StartGame()
        {
            if (isStarted) return;
            rect.localPosition = new Vector3(0, backgroundY1, 0);
            Destroy(tittle);
            isStarted = true;
        }

        public void LoadGame()
        {
			CloseSelf();
            AudioKit.Settings.MusicVolume.Value = Main.Interface.GetModel<GameData>().music / 100.0f;
            AudioKit.Settings.SoundVolume.Value = Main.Interface.GetModel<GameData>().sound / 100.0f;
            AudioKit.PlaySound(Main.Interface.GetModel<GameData>().soundPath + "Button" + Random.Range(1, 4).ToString());
            AudioKit.PlayMusic(gameData.musicPath + gameData.musicName[Random.Range(0, gameData.musicName.Length)], false);
			ActionKit.OnUpdate.Register(() =>
			{
				if (!AudioKit.MusicPlayer.AudioSource.isPlaying)
					AudioKit.PlayMusic(gameData.musicPath + gameData.musicName[Random.Range(0, gameData.musicName.Length)], false);
			});
            SceneManager.LoadScene("PlotPreface");
        }

		public void ExitGame()
		{
            CloseSelf();
            AudioKit.PlaySound(Main.Interface.GetModel<GameData>().soundPath + "Button" + Random.Range(1, 4).ToString());
			SceneManager.LoadScene("PlotEnd");
		}
    }
}
