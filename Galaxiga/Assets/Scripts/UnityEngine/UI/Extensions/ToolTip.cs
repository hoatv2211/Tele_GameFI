using System;

namespace UnityEngine.UI.Extensions
{
	[RequireComponent(typeof(RectTransform))]
	[AddComponentMenu("UI/Extensions/Tooltip")]
	public class ToolTip : MonoBehaviour
	{
		public void Awake()
		{
			Canvas componentInParent = base.GetComponentInParent<Canvas>();
			this._guiCamera = componentInParent.worldCamera;
			this._guiMode = componentInParent.renderMode;
			this._rectTransform = base.GetComponent<RectTransform>();
			this._text = base.GetComponentInChildren<Text>();
			this._inside = false;
			this.xShift = 0f;
			this.YShift = -30f;
			base.gameObject.SetActive(false);
		}

		public void SetTooltip(string ttext)
		{
			if (this._guiMode == RenderMode.ScreenSpaceCamera)
			{
				this._text.text = ttext;
				this._rectTransform.sizeDelta = new Vector2(this._text.preferredWidth + 40f, this._text.preferredHeight + 25f);
				this.OnScreenSpaceCamera();
			}
		}

		public void HideTooltip()
		{
			if (this._guiMode == RenderMode.ScreenSpaceCamera)
			{
				base.gameObject.SetActive(false);
				this._inside = false;
			}
		}

		private void FixedUpdate()
		{
			if (this._inside && this._guiMode == RenderMode.ScreenSpaceCamera)
			{
				this.OnScreenSpaceCamera();
			}
		}

		public void OnScreenSpaceCamera()
		{
			Vector3 position = this._guiCamera.ScreenToViewportPoint(UnityEngine.Input.mousePosition - new Vector3(this.xShift, this.YShift, 0f));
			Vector3 vector = this._guiCamera.ViewportToWorldPoint(position);
			this.width = this._rectTransform.sizeDelta[0];
			this.height = this._rectTransform.sizeDelta[1];
			Vector3 vector2 = this._guiCamera.ViewportToWorldPoint(new Vector3(0f, 0f, 0f));
			Vector3 vector3 = this._guiCamera.ViewportToWorldPoint(new Vector3(1f, 1f, 0f));
			float num = vector.x + this.width / 2f;
			if (num > vector3.x)
			{
				Vector3 vector4 = new Vector3(num - vector3.x, 0f, 0f);
				Vector3 position2 = new Vector3(vector.x - vector4.x, position.y, 0f);
				position.x = this._guiCamera.WorldToViewportPoint(position2).x;
			}
			num = vector.x - this.width / 2f;
			if (num < vector2.x)
			{
				Vector3 vector5 = new Vector3(vector2.x - num, 0f, 0f);
				Vector3 position3 = new Vector3(vector.x + vector5.x, position.y, 0f);
				position.x = this._guiCamera.WorldToViewportPoint(position3).x;
			}
			num = vector.y + this.height / 2f;
			if (num > vector3.y)
			{
				Vector3 vector6 = new Vector3(0f, 35f + this.height / 2f, 0f);
				Vector3 position4 = new Vector3(position.x, vector.y - vector6.y, 0f);
				position.y = this._guiCamera.WorldToViewportPoint(position4).y;
			}
			num = vector.y - this.height / 2f;
			if (num < vector2.y)
			{
				Vector3 vector7 = new Vector3(0f, 35f + this.height / 2f, 0f);
				Vector3 position5 = new Vector3(position.x, vector.y + vector7.y, 0f);
				position.y = this._guiCamera.WorldToViewportPoint(position5).y;
			}
			base.transform.position = new Vector3(vector.x, vector.y, 0f);
			base.gameObject.SetActive(true);
			this._inside = true;
		}

		private Text _text;

		private RectTransform _rectTransform;

		private bool _inside;

		private float width;

		private float height;

		private float YShift;

		private float xShift;

		private RenderMode _guiMode;

		private Camera _guiCamera;
	}
}
