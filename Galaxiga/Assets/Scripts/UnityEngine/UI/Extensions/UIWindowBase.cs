using System;
using UnityEngine.EventSystems;

namespace UnityEngine.UI.Extensions
{
	[RequireComponent(typeof(RectTransform))]
	[AddComponentMenu("UI/Extensions/UI Window Base")]
	public class UIWindowBase : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IEventSystemHandler
	{
		private void Start()
		{
			this.m_transform = base.GetComponent<RectTransform>();
			this.m_originalCoods = this.m_transform.position;
			this.m_canvas = base.GetComponentInParent<Canvas>();
			this.m_canvasRectTransform = this.m_canvas.GetComponent<RectTransform>();
		}

		private void Update()
		{
			if (UIWindowBase.ResetCoords)
			{
				this.resetCoordinatePosition();
			}
		}

		public void OnDrag(PointerEventData eventData)
		{
			if (this._isDragging)
			{
				Vector3 b = this.ScreenToCanvas(eventData.position) - this.ScreenToCanvas(eventData.position - eventData.delta);
				this.m_transform.localPosition += b;
			}
		}

		public void OnBeginDrag(PointerEventData eventData)
		{
			if (eventData.pointerCurrentRaycast.gameObject == null)
			{
				return;
			}
			if (eventData.pointerCurrentRaycast.gameObject.name == base.name)
			{
				this._isDragging = true;
			}
		}

		public void OnEndDrag(PointerEventData eventData)
		{
			this._isDragging = false;
		}

		private void resetCoordinatePosition()
		{
			this.m_transform.position = this.m_originalCoods;
			UIWindowBase.ResetCoords = false;
		}

		private Vector3 ScreenToCanvas(Vector3 screenPosition)
		{
			Vector2 sizeDelta = this.m_canvasRectTransform.sizeDelta;
			Vector3 result;
			Vector2 vector;
			Vector2 vector2;
			if (this.m_canvas.renderMode == RenderMode.ScreenSpaceOverlay || (this.m_canvas.renderMode == RenderMode.ScreenSpaceCamera && this.m_canvas.worldCamera == null))
			{
				result = screenPosition;
				vector = Vector2.zero;
				vector2 = sizeDelta;
			}
			else
			{
				Ray ray = this.m_canvas.worldCamera.ScreenPointToRay(screenPosition);
				Plane plane = new Plane(this.m_canvasRectTransform.forward, this.m_canvasRectTransform.position);
				float d;
				if (!plane.Raycast(ray, out d))
				{
					throw new Exception("Is it practically possible?");
				}
				Vector3 position = ray.origin + ray.direction * d;
				result = this.m_canvasRectTransform.InverseTransformPoint(position);
				vector = -Vector2.Scale(sizeDelta, this.m_canvasRectTransform.pivot);
				vector2 = Vector2.Scale(sizeDelta, Vector2.one - this.m_canvasRectTransform.pivot);
			}
			result.x = Mathf.Clamp(result.x, vector.x + (float)this.KeepWindowInCanvas, vector2.x - (float)this.KeepWindowInCanvas);
			result.y = Mathf.Clamp(result.y, vector.y + (float)this.KeepWindowInCanvas, vector2.y - (float)this.KeepWindowInCanvas);
			return result;
		}

		private RectTransform m_transform;

		private bool _isDragging;

		public static bool ResetCoords;

		private Vector3 m_originalCoods = Vector3.zero;

		private Canvas m_canvas;

		private RectTransform m_canvasRectTransform;

		public int KeepWindowInCanvas = 5;
	}
}
