using UnityEngine;
using UnityEngine.UI;
using QFramework;

namespace ns
{
	public class PlayerUIData : UIPanelData
	{
	}
	public partial class PlayerUI : UIPanel
	{
		protected override void OnInit(IUIData uiData = null)
		{
			mData = uiData as PlayerUIData ?? new PlayerUIData();
			// please add init code here
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
	}
}
