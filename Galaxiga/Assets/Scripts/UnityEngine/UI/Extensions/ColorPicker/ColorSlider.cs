using System;
using UnityEngine.Events;

namespace UnityEngine.UI.Extensions.ColorPicker
{
	[RequireComponent(typeof(Slider))]
	public class ColorSlider : MonoBehaviour
	{
		private void Awake()
		{
			this.slider = base.GetComponent<Slider>();
			this.ColorPicker.onValueChanged.AddListener(new UnityAction<Color>(this.ColorChanged));
			this.ColorPicker.onHSVChanged.AddListener(new UnityAction<float, float, float>(this.HSVChanged));
			this.slider.onValueChanged.AddListener(new UnityAction<float>(this.SliderChanged));
		}

		private void OnDestroy()
		{
			this.ColorPicker.onValueChanged.RemoveListener(new UnityAction<Color>(this.ColorChanged));
			this.ColorPicker.onHSVChanged.RemoveListener(new UnityAction<float, float, float>(this.HSVChanged));
			this.slider.onValueChanged.RemoveListener(new UnityAction<float>(this.SliderChanged));
		}

		private void ColorChanged(Color newColor)
		{
			this.listen = false;
			switch (this.type)
			{
			case ColorValues.R:
				this.slider.normalizedValue = newColor.r;
				break;
			case ColorValues.G:
				this.slider.normalizedValue = newColor.g;
				break;
			case ColorValues.B:
				this.slider.normalizedValue = newColor.b;
				break;
			case ColorValues.A:
				this.slider.normalizedValue = newColor.a;
				break;
			}
		}

		private void HSVChanged(float hue, float saturation, float value)
		{
			this.listen = false;
			switch (this.type)
			{
			case ColorValues.Hue:
				this.slider.normalizedValue = hue;
				break;
			case ColorValues.Saturation:
				this.slider.normalizedValue = saturation;
				break;
			case ColorValues.Value:
				this.slider.normalizedValue = value;
				break;
			}
		}

		private void SliderChanged(float newValue)
		{
			if (this.listen)
			{
				newValue = this.slider.normalizedValue;
				this.ColorPicker.AssignColor(this.type, newValue);
			}
			this.listen = true;
		}

		public ColorPickerControl ColorPicker;

		public ColorValues type;

		private Slider slider;

		private bool listen = true;
	}
}
