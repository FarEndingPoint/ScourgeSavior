using System;
using UnityEngine;
using UnityEngine.UI;
using QFramework;

namespace ns
{
	// Generate Id:e2bc7683-fad9-4f9f-aa58-f58c0ef0c00b
	public partial class MainMenuPanel
	{
		public const string Name = "MainMenuPanel";
		
		
		private MainMenuPanelData mPrivateData = null;
		
		protected override void ClearUIComponents()
		{
			
			mData = null;
		}
		
		public MainMenuPanelData Data
		{
			get
			{
				return mData;
			}
		}
		
		MainMenuPanelData mData
		{
			get
			{
				return mPrivateData ?? (mPrivateData = new MainMenuPanelData());
			}
			set
			{
				mUIData = value;
				mPrivateData = value;
			}
		}
	}
}
