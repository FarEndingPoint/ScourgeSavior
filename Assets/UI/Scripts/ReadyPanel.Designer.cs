using System;
using UnityEngine;
using UnityEngine.UI;
using QFramework;

namespace ns
{
	// Generate Id:409a7e96-573e-4f5a-a936-ce45a08edd09
	public partial class ReadyPanel
	{
		public const string Name = "ReadyPanel";
		
		
		private ReadyPanelData mPrivateData = null;
		
		protected override void ClearUIComponents()
		{
			
			mData = null;
		}
		
		public ReadyPanelData Data
		{
			get
			{
				return mData;
			}
		}
		
		ReadyPanelData mData
		{
			get
			{
				return mPrivateData ?? (mPrivateData = new ReadyPanelData());
			}
			set
			{
				mUIData = value;
				mPrivateData = value;
			}
		}
	}
}
