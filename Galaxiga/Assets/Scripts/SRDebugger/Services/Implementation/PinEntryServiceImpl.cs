using System;
using System.Collections.Generic;
using System.Linq;
using SRDebugger.Internal;
using SRDebugger.UI.Controls;
using SRF;
using SRF.Service;
using UnityEngine;

namespace SRDebugger.Services.Implementation
{
	[Service(typeof(IPinEntryService))]
	public class PinEntryServiceImpl : SRServiceBase<IPinEntryService>, IPinEntryService
	{
		public bool IsShowingKeypad
		{
			get
			{
				return this._isVisible;
			}
		}

		public void ShowPinEntry(IList<int> requiredPin, string message, PinEntryCompleteCallback callback, bool allowCancel = true)
		{
			if (this._isVisible)
			{
				throw new InvalidOperationException("Pin entry is already in progress");
			}
			this.VerifyPin(requiredPin);
			if (this._pinControl == null)
			{
				this.Load();
			}
			if (this._pinControl == null)
			{
				UnityEngine.Debug.LogWarning("[PinEntry] Pin entry failed loading, executing callback with fail result");
				callback(false);
				return;
			}
			this._pinControl.Clear();
			this._pinControl.PromptText.text = message;
			this._pinControl.CanCancel = allowCancel;
			this._callback = callback;
			this._requiredPin.Clear();
			this._requiredPin.AddRange(requiredPin);
			this._pinControl.Show();
			this._isVisible = true;
			SRDebuggerUtil.EnsureEventSystemExists();
		}

		[Obsolete]
		public void ShowPinEntry(IList<int> requiredPin, string message, PinEntryCompleteCallback callback, bool blockInput, bool allowCancel)
		{
			this.ShowPinEntry(requiredPin, message, callback, allowCancel);
		}

		protected override void Awake()
		{
			base.Awake();
			base.CachedTransform.SetParent(Hierarchy.Get("SRDebugger"));
		}

		private void Load()
		{
			PinEntryControl pinEntryControl = Resources.Load<PinEntryControl>("SRDebugger/UI/Prefabs/PinEntry");
			if (pinEntryControl == null)
			{
				UnityEngine.Debug.LogError("[PinEntry] Unable to load pin entry prefab");
				return;
			}
			this._pinControl = SRInstantiate.Instantiate<PinEntryControl>(pinEntryControl);
			this._pinControl.CachedTransform.SetParent(base.CachedTransform, false);
			this._pinControl.Hide();
			this._pinControl.Complete += this.PinControlOnComplete;
		}

		private void PinControlOnComplete(IList<int> result, bool didCancel)
		{
			bool flag = this._requiredPin.SequenceEqual(result);
			if (!didCancel && !flag)
			{
				this._pinControl.Clear();
				this._pinControl.PlayInvalidCodeAnimation();
				return;
			}
			this._isVisible = false;
			this._pinControl.Hide();
			if (didCancel)
			{
				this._callback(false);
				return;
			}
			this._callback(flag);
		}

		private void VerifyPin(IList<int> pin)
		{
			if (pin.Count != 4)
			{
				throw new ArgumentException("Pin list must have 4 elements");
			}
			for (int i = 0; i < pin.Count; i++)
			{
				if (pin[i] < 0 || pin[i] > 9)
				{
					throw new ArgumentException("Pin numbers must be >= 0 && <= 9");
				}
			}
		}

		private PinEntryCompleteCallback _callback;

		private bool _isVisible;

		private PinEntryControl _pinControl;

		private List<int> _requiredPin = new List<int>(4);
	}
}
