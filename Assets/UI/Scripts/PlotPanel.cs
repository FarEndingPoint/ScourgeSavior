using UnityEngine;
using UnityEngine.UI;
using QFramework;
using DG.Tweening;

namespace ns
{
	public class PlotPanelData : UIPanelData
	{
	}
	public partial class PlotPanel : UIPanel
	{
		[SerializeField] Text plot1, plot2;

		protected override void OnInit(IUIData uiData = null)
		{
			mData = uiData as PlotPanelData ?? new PlotPanelData();
            // please add init code here
        }
		
		protected override void OnOpen(IUIData uiData = null)
		{
            plot1.DOText("How to save Kyhra against loneliness if the young girl has become the pilot?", 5);
            ActionKit.Delay(6, () =>
            {
                plot2.DOText("Maybe another warrior?", 2.5f);
            }).Start(this);
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
