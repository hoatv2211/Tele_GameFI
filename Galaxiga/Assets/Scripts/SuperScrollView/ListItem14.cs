using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace SuperScrollView
{
	public class ListItem14 : MonoBehaviour
	{
		public void Init()
		{
			int childCount = base.transform.childCount;
			for (int i = 0; i < childCount; i++)
			{
				Transform child = base.transform.GetChild(i);
				ListItem14Elem listItem14Elem = new ListItem14Elem();
				listItem14Elem.mRootObj = child.gameObject;
				listItem14Elem.mIcon = child.Find("ItemIcon").GetComponent<Image>();
				listItem14Elem.mName = child.Find("ItemIcon/name").GetComponent<Text>();
				this.mElemItemList.Add(listItem14Elem);
			}
		}

		public List<ListItem14Elem> mElemItemList = new List<ListItem14Elem>();
	}
}
