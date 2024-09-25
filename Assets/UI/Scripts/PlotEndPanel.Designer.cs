using System;
using UnityEngine;
using UnityEngine.UI;
using QFramework;

namespace ns
{
	// Generate Id:2ba761d2-9f1f-4504-9016-b6858e760df6
	public partial class PlotEndPanel
	{
		public const string Name = "PlotEndPanel";
		
		
		private PlotEndPanelData mPrivateData = null;
		
		protected override void ClearUIComponents()
		{
			
			mData = null;
		}
		
		public PlotEndPanelData Data
		{
			get
			{
				return mData;
			}
		}
		
		PlotEndPanelData mData
		{
			get
			{
				return mPrivateData ?? (mPrivateData = new PlotEndPanelData());
			}
			set
			{
				mUIData = value;
				mPrivateData = value;
			}
		}
	}
}
