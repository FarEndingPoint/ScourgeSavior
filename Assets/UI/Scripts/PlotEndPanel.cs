using UnityEngine;
using UnityEngine.UI;
using QFramework;
using DG.Tweening;

namespace ns
{
	public class PlotEndPanelData : UIPanelData
	{
	}
	public partial class PlotEndPanel : UIPanel
	{
		[SerializeField] Text plot;

		protected override void OnInit(IUIData uiData = null)
		{
			mData = uiData as PlotEndPanelData ?? new PlotEndPanelData();
			// please add init code here
		}
		
		protected override void OnOpen(IUIData uiData = null)
		{
            plot.DOText("Who will be next scourge bringer?", 4);
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
