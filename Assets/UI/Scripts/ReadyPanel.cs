using UnityEngine;
using UnityEngine.UI;
using QFramework;

namespace ns
{
	public class ReadyPanelData : UIPanelData
	{
	}
	public partial class ReadyPanel : UIPanel
	{
		protected override void OnInit(IUIData uiData = null)
		{
			mData = uiData as ReadyPanelData ?? new ReadyPanelData();
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
