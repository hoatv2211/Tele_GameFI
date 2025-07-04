using System;
using UnityEngine.EventSystems;

namespace UnityEngine.UI.Extensions
{
	[RequireComponent(typeof(Image))]
	[AddComponentMenu("UI/Extensions/UI_Knob")]
	public class UI_Knob : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IPointerEnterHandler, IPointerExitHandler, IBeginDragHandler, IDragHandler, IEventSystemHandler
	{
		public void OnPointerDown(PointerEventData eventData)
		{
			this._canDrag = true;
		}

		public void OnPointerUp(PointerEventData eventData)
		{
			this._canDrag = false;
		}

		public void OnPointerEnter(PointerEventData eventData)
		{
			this._canDrag = true;
		}

		public void OnPointerExit(PointerEventData eventData)
		{
			this._canDrag = false;
		}

		public void OnBeginDrag(PointerEventData eventData)
		{
			this.SetInitPointerData(eventData);
		}

		private void SetInitPointerData(PointerEventData eventData)
		{
            _initRotation = base.transform.rotation;
            _currentVector = eventData.position - (Vector2)base.transform.position;
            _initAngle = Mathf.Atan2(_currentVector.y, _currentVector.x) * 57.29578f;
        }

		public void OnDrag(PointerEventData eventData)
		{
            if (!_canDrag)
            {
                SetInitPointerData(eventData);
                return;
            }
            _currentVector = eventData.position - (Vector2)base.transform.position;
            _currentAngle = Mathf.Atan2(_currentVector.y, _currentVector.x) * 57.29578f;
            Quaternion quaternion = Quaternion.AngleAxis(_currentAngle - _initAngle, base.transform.forward);
            quaternion.eulerAngles = new Vector3(0f, 0f, quaternion.eulerAngles.z);
            Quaternion rotation = _initRotation * quaternion;
            if (direction == Direction.CW)
            {
                knobValue = 1f - rotation.eulerAngles.z / 360f;
                if (snapToPosition)
                {
                    SnapToPosition(ref knobValue);
                    rotation.eulerAngles = new Vector3(0f, 0f, 360f - 360f * knobValue);
                }
            }
            else
            {
                knobValue = rotation.eulerAngles.z / 360f;
                if (snapToPosition)
                {
                    SnapToPosition(ref knobValue);
                    rotation.eulerAngles = new Vector3(0f, 0f, 360f * knobValue);
                }
            }
            if (Mathf.Abs(knobValue - _previousValue) > 0.5f)
            {
                if (knobValue < 0.5f && loops > 1 && _currentLoops < (float)(loops - 1))
                {
                    _currentLoops += 1f;
                }
                else if (knobValue > 0.5f && _currentLoops >= 1f)
                {
                    _currentLoops -= 1f;
                }
                else
                {
                    if (knobValue > 0.5f && _currentLoops == 0f)
                    {
                        knobValue = 0f;
                        base.transform.localEulerAngles = Vector3.zero;
                        SetInitPointerData(eventData);
                        InvokeEvents(knobValue + _currentLoops);
                        return;
                    }
                    if (knobValue < 0.5f && _currentLoops == (float)(loops - 1))
                    {
                        knobValue = 1f;
                        base.transform.localEulerAngles = Vector3.zero;
                        SetInitPointerData(eventData);
                        InvokeEvents(knobValue + _currentLoops);
                        return;
                    }
                }
            }
            if (maxValue > 0f && knobValue + _currentLoops > maxValue)
            {
                knobValue = maxValue;
                float z = ((direction != 0) ? (360f * maxValue) : (360f - 360f * maxValue));
                base.transform.localEulerAngles = new Vector3(0f, 0f, z);
                SetInitPointerData(eventData);
                InvokeEvents(knobValue);
            }
            else
            {
                base.transform.rotation = rotation;
                InvokeEvents(knobValue + _currentLoops);
                _previousValue = knobValue;
            }
        }

		private void SnapToPosition(ref float knobValue)
		{
			float num = 1f / (float)this.snapStepsPerLoop;
			float num2 = Mathf.Round(knobValue / num) * num;
			knobValue = num2;
		}

		private void InvokeEvents(float value)
		{
			if (this.clampOutput01)
			{
				value /= (float)this.loops;
			}
			this.OnValueChanged.Invoke(value);
		}

		[Tooltip("Direction of rotation CW - clockwise, CCW - counterClockwise")]
		public UI_Knob.Direction direction;

		[HideInInspector]
		public float knobValue;

		[Tooltip("Max value of the knob, maximum RAW output value knob can reach, overrides snap step, IF set to 0 or higher than loops, max value will be set by loops")]
		public float maxValue;

		[Tooltip("How many rotations knob can do, if higher than max value, the latter will limit max value")]
		public int loops = 1;

		[Tooltip("Clamp output value between 0 and 1, usefull with loops > 1")]
		public bool clampOutput01;

		[Tooltip("snap to position?")]
		public bool snapToPosition;

		[Tooltip("Number of positions to snap")]
		public int snapStepsPerLoop = 10;

		[Space(30f)]
		public KnobFloatValueEvent OnValueChanged;

		private float _currentLoops;

		private float _previousValue;

		private float _initAngle;

		private float _currentAngle;

		private Vector2 _currentVector;

		private Quaternion _initRotation;

		private bool _canDrag;

		public enum Direction
		{
			CW,
			CCW
		}
	}
}
