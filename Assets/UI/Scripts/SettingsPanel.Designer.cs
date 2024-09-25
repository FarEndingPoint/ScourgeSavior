using System;
using UnityEngine;
using UnityEngine.UI;
using QFramework;

namespace ns
{
	// Generate Id:a39c93b7-f179-4614-a18a-1f9fd279c116
	public partial class SettingsPanel
	{
		public const string Name = "SettingsPanel";
		
		
		private SettingsPanelData mPrivateData = null;
		
		protected override void ClearUIComponents()
		{
			
			mData = null;
		}
		
		public SettingsPanelData Data
		{
			get
			{
				return mData;
			}
		}
		
		SettingsPanelData mData
		{
			get
			{
				return mPrivateData ?? (mPrivateData = new SettingsPanelData());
			}
			set
			{
				mUIData = value;
				mPrivateData = value;
			}
		}
	}
}
