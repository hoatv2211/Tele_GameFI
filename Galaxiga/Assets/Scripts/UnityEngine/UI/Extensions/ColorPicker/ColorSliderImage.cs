using System;
using UnityEngine.Events;

namespace UnityEngine.UI.Extensions.ColorPicker
{
	[RequireComponent(typeof(RawImage))]
	[ExecuteInEditMode]
	public class ColorSliderImage : MonoBehaviour
	{
		private RectTransform RectTransform
		{
			get
			{
				return base.transform as RectTransform;
			}
		}

		private void Awake()
		{
			this.image = base.GetComponent<RawImage>();
			if (this.image)
			{
				this.RegenerateTexture();
			}
			else
			{
				UnityEngine.Debug.LogWarning("Missing RawImage on object [" + base.name + "]");
			}
		}

		private void OnEnable()
		{
			if (this.picker != null && Application.isPlaying)
			{
				this.picker.onValueChanged.AddListener(new UnityAction<Color>(this.ColorChanged));
				this.picker.onHSVChanged.AddListener(new UnityAction<float, float, float>(this.ColorChanged));
			}
		}

		private void OnDisable()
		{
			if (this.picker != null)
			{
				this.picker.onValueChanged.RemoveListener(new UnityAction<Color>(this.ColorChanged));
				this.picker.onHSVChanged.RemoveListener(new UnityAction<float, float, float>(this.ColorChanged));
			}
		}

		private void OnDestroy()
		{
			if (this.image.texture != null)
			{
				UnityEngine.Object.DestroyImmediate(this.image.texture);
			}
		}

		private void ColorChanged(Color newColor)
		{
			switch (this.type)
			{
			case ColorValues.R:
			case ColorValues.G:
			case ColorValues.B:
			case ColorValues.Saturation:
			case ColorValues.Value:
				this.RegenerateTexture();
				break;
			}
		}

		private void ColorChanged(float hue, float saturation, float value)
		{
			switch (this.type)
			{
			case ColorValues.R:
			case ColorValues.G:
			case ColorValues.B:
			case ColorValues.Saturation:
			case ColorValues.Value:
				this.RegenerateTexture();
				break;
			}
		}

		private void RegenerateTexture()
		{
			if (!this.picker)
			{
				UnityEngine.Debug.LogWarning("Missing Picker on object [" + base.name + "]");
			}
			Color32 color = (!(this.picker != null)) ? Color.black : this.picker.CurrentColor;
			float num = (!(this.picker != null)) ? 0f : this.picker.H;
			float num2 = (!(this.picker != null)) ? 0f : this.picker.S;
			float num3 = (!(this.picker != null)) ? 0f : this.picker.V;
			bool flag = this.direction == Slider.Direction.BottomToTop || this.direction == Slider.Direction.TopToBottom;
			bool flag2 = this.direction == Slider.Direction.TopToBottom || this.direction == Slider.Direction.RightToLeft;
			int num4;
			switch (this.type)
			{
			case ColorValues.R:
			case ColorValues.G:
			case ColorValues.B:
			case ColorValues.A:
				num4 = 255;
				break;
			case ColorValues.Hue:
				num4 = 360;
				break;
			case ColorValues.Saturation:
			case ColorValues.Value:
				num4 = 100;
				break;
			default:
				throw new NotImplementedException(string.Empty);
			}
			Texture2D texture2D;
			if (flag)
			{
				texture2D = new Texture2D(1, num4);
			}
			else
			{
				texture2D = new Texture2D(num4, 1);
			}
			texture2D.hideFlags = HideFlags.DontSave;
			Color32[] array = new Color32[num4];
			switch (this.type)
			{
			case ColorValues.R:
			{
				byte b = 0;
				while ((int)b < num4)
				{
					array[(!flag2) ? ((int)b) : (num4 - 1 - (int)b)] = new Color32(b, color.g, color.b, byte.MaxValue);
					b += 1;
				}
				break;
			}
			case ColorValues.G:
			{
				byte b2 = 0;
				while ((int)b2 < num4)
				{
					array[(!flag2) ? ((int)b2) : (num4 - 1 - (int)b2)] = new Color32(color.r, b2, color.b, byte.MaxValue);
					b2 += 1;
				}
				break;
			}
			case ColorValues.B:
			{
				byte b3 = 0;
				while ((int)b3 < num4)
				{
					array[(!flag2) ? ((int)b3) : (num4 - 1 - (int)b3)] = new Color32(color.r, color.g, b3, byte.MaxValue);
					b3 += 1;
				}
				break;
			}
			case ColorValues.A:
			{
				byte b4 = 0;
				while ((int)b4 < num4)
				{
					array[(!flag2) ? ((int)b4) : (num4 - 1 - (int)b4)] = new Color32(b4, b4, b4, byte.MaxValue);
					b4 += 1;
				}
				break;
			}
			case ColorValues.Hue:
				for (int i = 0; i < num4; i++)
				{
					array[(!flag2) ? i : (num4 - 1 - i)] = HSVUtil.ConvertHsvToRgb((double)i, 1.0, 1.0, 1f);
				}
				break;
			case ColorValues.Saturation:
				for (int j = 0; j < num4; j++)
				{
					array[(!flag2) ? j : (num4 - 1 - j)] = HSVUtil.ConvertHsvToRgb((double)(num * 360f), (double)((float)j / (float)num4), (double)num3, 1f);
				}
				break;
			case ColorValues.Value:
				for (int k = 0; k < num4; k++)
				{
					array[(!flag2) ? k : (num4 - 1 - k)] = HSVUtil.ConvertHsvToRgb((double)(num * 360f), (double)num2, (double)((float)k / (float)num4), 1f);
				}
				break;
			default:
				throw new NotImplementedException(string.Empty);
			}
			texture2D.SetPixels32(array);
			texture2D.Apply();
			if (this.image.texture != null)
			{
				UnityEngine.Object.DestroyImmediate(this.image.texture);
			}
			this.image.texture = texture2D;
			switch (this.direction)
			{
			case Slider.Direction.LeftToRight:
			case Slider.Direction.RightToLeft:
				this.image.uvRect = new Rect(0f, 0f, 1f, 2f);
				break;
			case Slider.Direction.BottomToTop:
			case Slider.Direction.TopToBottom:
				this.image.uvRect = new Rect(0f, 0f, 2f, 1f);
				break;
			}
		}

		public ColorPickerControl picker;

		public ColorValues type;

		public Slider.Direction direction;

		private RawImage image;
	}
}
