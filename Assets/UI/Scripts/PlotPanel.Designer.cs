using System;
using UnityEngine;
using UnityEngine.UI;
using QFramework;

namespace ns
{
	// Generate Id:9ae5444c-c55e-43ed-90c7-58a4ab4046f5
	public partial class PlotPanel
	{
		public const string Name = "PlotPanel";
		
		
		private PlotPanelData mPrivateData = null;
		
		protected override void ClearUIComponents()
		{
			
			mData = null;
		}
		
		public PlotPanelData Data
		{
			get
			{
				return mData;
			}
		}
		
		PlotPanelData mData
		{
			get
			{
				return mPrivateData ?? (mPrivateData = new PlotPanelData());
			}
			set
			{
				mUIData = value;
				mPrivateData = value;
			}
		}
	}
}
