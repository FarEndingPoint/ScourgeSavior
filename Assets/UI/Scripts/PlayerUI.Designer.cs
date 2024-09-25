using System;
using UnityEngine;
using UnityEngine.UI;
using QFramework;

namespace ns
{
	// Generate Id:342deb09-2f47-4745-b679-dbf3e981b151
	public partial class PlayerUI
	{
		public const string Name = "PlayerUI";
		
		
		private PlayerUIData mPrivateData = null;
		
		protected override void ClearUIComponents()
		{
			
			mData = null;
		}
		
		public PlayerUIData Data
		{
			get
			{
				return mData;
			}
		}
		
		PlayerUIData mData
		{
			get
			{
				return mPrivateData ?? (mPrivateData = new PlayerUIData());
			}
			set
			{
				mUIData = value;
				mPrivateData = value;
			}
		}
	}
}
