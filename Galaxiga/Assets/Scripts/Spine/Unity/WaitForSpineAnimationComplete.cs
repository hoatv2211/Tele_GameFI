using System;
using System.Collections;
using UnityEngine;

namespace Spine.Unity
{
	public class WaitForSpineAnimationComplete : IEnumerator
	{
		public WaitForSpineAnimationComplete(TrackEntry trackEntry)
		{
			this.SafeSubscribe(trackEntry);
		}

		private void HandleComplete(TrackEntry trackEntry)
		{
			this.m_WasFired = true;
		}

		private void SafeSubscribe(TrackEntry trackEntry)
		{
			if (trackEntry == null)
			{
				UnityEngine.Debug.LogWarning("TrackEntry was null. Coroutine will continue immediately.");
				this.m_WasFired = true;
			}
			else
			{
				trackEntry.Complete += this.HandleComplete;
			}
		}

		public WaitForSpineAnimationComplete NowWaitFor(TrackEntry trackEntry)
		{
			this.SafeSubscribe(trackEntry);
			return this;
		}

		bool IEnumerator.MoveNext()
		{
			if (this.m_WasFired)
			{
				((IEnumerator)this).Reset();
				return false;
			}
			return true;
		}

		void IEnumerator.Reset()
		{
			this.m_WasFired = false;
		}

		object IEnumerator.Current
		{
			get
			{
				return null;
			}
		}

		private bool m_WasFired;
	}
}
