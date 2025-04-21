using System;
using System.Collections.Generic;

namespace UnityEngine.UI.Extensions
{
	[AddComponentMenu("UI/Effects/Extensions/Gradient2")]
	public class Gradient2 : BaseMeshEffect
	{
		public Gradient2.Blend BlendMode
		{
			get
			{
				return this._blendMode;
			}
			set
			{
				this._blendMode = value;
				base.graphic.SetVerticesDirty();
			}
		}

		public UnityEngine.Gradient EffectGradient
		{
			get
			{
				return this._effectGradient;
			}
			set
			{
				this._effectGradient = value;
				base.graphic.SetVerticesDirty();
			}
		}

		public Gradient2.Type GradientType
		{
			get
			{
				return this._gradientType;
			}
			set
			{
				this._gradientType = value;
				base.graphic.SetVerticesDirty();
			}
		}

		public float Offset
		{
			get
			{
				return this._offset;
			}
			set
			{
				this._offset = value;
				base.graphic.SetVerticesDirty();
			}
		}

		public override void ModifyMesh(VertexHelper helper)
		{
			if (!this.IsActive() || helper.currentVertCount == 0)
			{
				return;
			}
			List<UIVertex> list = new List<UIVertex>();
			helper.GetUIVertexStream(list);
			int count = list.Count;
			switch (this.GradientType)
			{
			case Gradient2.Type.Horizontal:
			{
				float num = list[0].position.x;
				float num2 = list[0].position.x;
				for (int i = count - 1; i >= 1; i--)
				{
					float x = list[i].position.x;
					if (x > num2)
					{
						num2 = x;
					}
					else if (x < num)
					{
						num = x;
					}
				}
				float num3 = 1f / (num2 - num);
				UIVertex vertex = default(UIVertex);
				for (int j = 0; j < helper.currentVertCount; j++)
				{
					helper.PopulateUIVertex(ref vertex, j);
					vertex.color = this.BlendColor(vertex.color, this.EffectGradient.Evaluate((vertex.position.x - num) * num3 - this.Offset));
					helper.SetUIVertex(vertex, j);
				}
				break;
			}
			case Gradient2.Type.Vertical:
			{
				float num4 = list[0].position.y;
				float num5 = list[0].position.y;
				for (int k = count - 1; k >= 1; k--)
				{
					float y = list[k].position.y;
					if (y > num5)
					{
						num5 = y;
					}
					else if (y < num4)
					{
						num4 = y;
					}
				}
				float num6 = 1f / (num5 - num4);
				UIVertex vertex2 = default(UIVertex);
				for (int l = 0; l < helper.currentVertCount; l++)
				{
					helper.PopulateUIVertex(ref vertex2, l);
					vertex2.color = this.BlendColor(vertex2.color, this.EffectGradient.Evaluate((vertex2.position.y - num4) * num6 - this.Offset));
					helper.SetUIVertex(vertex2, l);
				}
				break;
			}
			case Gradient2.Type.Radial:
			{
				float num7 = list[0].position.x;
				float num8 = list[0].position.x;
				float num9 = list[0].position.y;
				float num10 = list[0].position.y;
				for (int m = count - 1; m >= 1; m--)
				{
					float x2 = list[m].position.x;
					if (x2 > num8)
					{
						num8 = x2;
					}
					else if (x2 < num7)
					{
						num7 = x2;
					}
					float y2 = list[m].position.y;
					if (y2 > num10)
					{
						num10 = y2;
					}
					else if (y2 < num9)
					{
						num9 = y2;
					}
				}
				float num11 = 1f / (num8 - num7);
				float num12 = 1f / (num10 - num9);
				helper.Clear();
				float num13 = (num8 + num7) / 2f;
				float num14 = (num9 + num10) / 2f;
				float num15 = (num8 - num7) / 2f;
				float num16 = (num10 - num9) / 2f;
				UIVertex v = default(UIVertex);
				v.position = Vector3.right * num13 + Vector3.up * num14 + Vector3.forward * list[0].position.z;
				v.normal = list[0].normal;
				v.color = Color.white;
				int num17 = 64;
				for (int n = 0; n < num17; n++)
				{
					UIVertex v2 = default(UIVertex);
					float num18 = (float)n * 360f / (float)num17;
					float d = Mathf.Cos(0.0174532924f * num18) * num15;
					float d2 = Mathf.Sin(0.0174532924f * num18) * num16;
					v2.position = Vector3.right * d + Vector3.up * d2 + Vector3.forward * list[0].position.z;
					v2.normal = list[0].normal;
					v2.color = Color.white;
					helper.AddVert(v2);
				}
				helper.AddVert(v);
				for (int num19 = 1; num19 < num17; num19++)
				{
					helper.AddTriangle(num19 - 1, num19, num17);
				}
				helper.AddTriangle(0, num17 - 1, num17);
				UIVertex vertex3 = default(UIVertex);
				for (int num20 = 0; num20 < helper.currentVertCount; num20++)
				{
					helper.PopulateUIVertex(ref vertex3, num20);
					vertex3.color = this.BlendColor(vertex3.color, this.EffectGradient.Evaluate(Mathf.Sqrt(Mathf.Pow(Mathf.Abs(vertex3.position.x - num13) * num11, 2f) + Mathf.Pow(Mathf.Abs(vertex3.position.y - num14) * num12, 2f)) * 2f - this.Offset));
					helper.SetUIVertex(vertex3, num20);
				}
				break;
			}
			case Gradient2.Type.Diamond:
			{
				float num21 = list[0].position.y;
				float num22 = list[0].position.y;
				for (int num23 = count - 1; num23 >= 1; num23--)
				{
					float y3 = list[num23].position.y;
					if (y3 > num22)
					{
						num22 = y3;
					}
					else if (y3 < num21)
					{
						num21 = y3;
					}
				}
				float num24 = 1f / (num22 - num21);
				helper.Clear();
				for (int num25 = 0; num25 < count; num25++)
				{
					helper.AddVert(list[num25]);
				}
				float d3 = (num21 + num22) / 2f;
				UIVertex v3 = new UIVertex
				{
					position = (Vector3.right + Vector3.up) * d3 + Vector3.forward * list[0].position.z,
					normal = list[0].normal,
					color = Color.white
				};
				helper.AddVert(v3);
				for (int num26 = 1; num26 < count; num26++)
				{
					helper.AddTriangle(num26 - 1, num26, count);
				}
				helper.AddTriangle(0, count - 1, count);
				UIVertex vertex4 = default(UIVertex);
				for (int num27 = 0; num27 < helper.currentVertCount; num27++)
				{
					helper.PopulateUIVertex(ref vertex4, num27);
					vertex4.color = this.BlendColor(vertex4.color, this.EffectGradient.Evaluate(Vector3.Distance(vertex4.position, v3.position) * num24 - this.Offset));
					helper.SetUIVertex(vertex4, num27);
				}
				break;
			}
			}
		}

		private Color BlendColor(Color colorA, Color colorB)
		{
			Gradient2.Blend blendMode = this.BlendMode;
			if (blendMode == Gradient2.Blend.Add)
			{
				return colorA + colorB;
			}
			if (blendMode != Gradient2.Blend.Multiply)
			{
				return colorB;
			}
			return colorA * colorB;
		}

		[SerializeField]
		private Gradient2.Type _gradientType;

		[SerializeField]
		private Gradient2.Blend _blendMode = Gradient2.Blend.Multiply;

		[SerializeField]
		[Range(-1f, 1f)]
		private float _offset;

		[SerializeField]
		private UnityEngine.Gradient _effectGradient = new UnityEngine.Gradient
		{
			colorKeys = new GradientColorKey[]
			{
				new GradientColorKey(Color.black, 0f),
				new GradientColorKey(Color.white, 1f)
			}
		};

		public enum Type
		{
			Horizontal,
			Vertical,
			Radial,
			Diamond
		}

		public enum Blend
		{
			Override,
			Add,
			Multiply
		}
	}
}
