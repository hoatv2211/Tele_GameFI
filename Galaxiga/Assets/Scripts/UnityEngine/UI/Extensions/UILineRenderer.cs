using System;
using System.Collections.Generic;
using UnityEngine.Sprites;

namespace UnityEngine.UI.Extensions
{
	[AddComponentMenu("UI/Extensions/Primitives/UILineRenderer")]
	[RequireComponent(typeof(RectTransform))]
	public class UILineRenderer : UIPrimitiveBase
	{
		public float LineThickness
		{
			get
			{
				return this.lineThickness;
			}
			set
			{
				this.lineThickness = value;
				this.SetAllDirty();
			}
		}

		public bool RelativeSize
		{
			get
			{
				return this.relativeSize;
			}
			set
			{
				this.relativeSize = value;
				this.SetAllDirty();
			}
		}

		public bool LineList
		{
			get
			{
				return this.lineList;
			}
			set
			{
				this.lineList = value;
				this.SetAllDirty();
			}
		}

		public bool LineCaps
		{
			get
			{
				return this.lineCaps;
			}
			set
			{
				this.lineCaps = value;
				this.SetAllDirty();
			}
		}

		public int BezierSegmentsPerCurve
		{
			get
			{
				return this.bezierSegmentsPerCurve;
			}
			set
			{
				this.bezierSegmentsPerCurve = value;
			}
		}

		public Vector2[] Points
		{
			get
			{
				return this.m_points;
			}
			set
			{
				if (this.m_points == value)
				{
					return;
				}
				this.m_points = value;
				this.SetAllDirty();
			}
		}

		protected override void OnPopulateMesh(VertexHelper vh)
		{
			if (this.m_points == null)
			{
				return;
			}
			this.GeneratedUVs();
			Vector2[] array = this.m_points;
			if (this.BezierMode != UILineRenderer.BezierType.None && this.BezierMode != UILineRenderer.BezierType.Catenary && this.m_points.Length > 3)
			{
				BezierPath bezierPath = new BezierPath();
				bezierPath.SetControlPoints(array);
				bezierPath.SegmentsPerCurve = this.bezierSegmentsPerCurve;
				UILineRenderer.BezierType bezierMode = this.BezierMode;
				List<Vector2> list;
				if (bezierMode != UILineRenderer.BezierType.Basic)
				{
					if (bezierMode != UILineRenderer.BezierType.Improved)
					{
						list = bezierPath.GetDrawingPoints2();
					}
					else
					{
						list = bezierPath.GetDrawingPoints1();
					}
				}
				else
				{
					list = bezierPath.GetDrawingPoints0();
				}
				array = list.ToArray();
			}
			if (this.BezierMode == UILineRenderer.BezierType.Catenary && this.m_points.Length == 2)
			{
				array = new CableCurve(array)
				{
					slack = base.Resoloution,
					steps = this.BezierSegmentsPerCurve
				}.Points();
			}
			if (base.ImproveResolution != ResolutionMode.None)
			{
				array = base.IncreaseResolution(array);
			}
			float num = this.relativeSize ? base.rectTransform.rect.width : 1f;
			float num2 = this.relativeSize ? base.rectTransform.rect.height : 1f;
			float num3 = -base.rectTransform.pivot.x * num;
			float num4 = -base.rectTransform.pivot.y * num2;
			vh.Clear();
			List<UIVertex[]> list2 = new List<UIVertex[]>();
			if (this.lineList)
			{
				for (int i = 1; i < array.Length; i += 2)
				{
					Vector2 start = array[i - 1];
					Vector2 end = array[i];
					start = new Vector2(start.x * num + num3, start.y * num2 + num4);
					end = new Vector2(end.x * num + num3, end.y * num2 + num4);
					if (this.lineCaps)
					{
						list2.Add(this.CreateLineCap(start, end, UILineRenderer.SegmentType.Start));
					}
					list2.Add(this.CreateLineSegment(start, end, UILineRenderer.SegmentType.Middle));
					if (this.lineCaps)
					{
						list2.Add(this.CreateLineCap(start, end, UILineRenderer.SegmentType.End));
					}
				}
			}
			else
			{
				for (int j = 1; j < array.Length; j++)
				{
					Vector2 start2 = array[j - 1];
					Vector2 end2 = array[j];
					start2 = new Vector2(start2.x * num + num3, start2.y * num2 + num4);
					end2 = new Vector2(end2.x * num + num3, end2.y * num2 + num4);
					if (this.lineCaps && j == 1)
					{
						list2.Add(this.CreateLineCap(start2, end2, UILineRenderer.SegmentType.Start));
					}
					list2.Add(this.CreateLineSegment(start2, end2, UILineRenderer.SegmentType.Middle));
					if (this.lineCaps && j == array.Length - 1)
					{
						list2.Add(this.CreateLineCap(start2, end2, UILineRenderer.SegmentType.End));
					}
				}
			}
			for (int k = 0; k < list2.Count; k++)
			{
				if (!this.lineList && k < list2.Count - 1)
				{
					Vector3 v = list2[k][1].position - list2[k][2].position;
					Vector3 v2 = list2[k + 1][2].position - list2[k + 1][1].position;
					float num5 = Vector2.Angle(v, v2) * 0.0174532924f;
					float num6 = Mathf.Sign(Vector3.Cross(v.normalized, v2.normalized).z);
					float num7 = this.lineThickness / (2f * Mathf.Tan(num5 / 2f));
					Vector3 position = list2[k][2].position - v.normalized * num7 * num6;
					Vector3 position2 = list2[k][3].position + v.normalized * num7 * num6;
					UILineRenderer.JoinType joinType = this.LineJoins;
					if (joinType == UILineRenderer.JoinType.Miter)
					{
						if (num7 < v.magnitude / 2f && num7 < v2.magnitude / 2f && num5 > 0.2617994f)
						{
							list2[k][2].position = position;
							list2[k][3].position = position2;
							list2[k + 1][0].position = position2;
							list2[k + 1][1].position = position;
						}
						else
						{
							joinType = UILineRenderer.JoinType.Bevel;
						}
					}
					if (joinType == UILineRenderer.JoinType.Bevel)
					{
						if (num7 < v.magnitude / 2f && num7 < v2.magnitude / 2f && num5 > 0.5235988f)
						{
							if (num6 < 0f)
							{
								list2[k][2].position = position;
								list2[k + 1][1].position = position;
							}
							else
							{
								list2[k][3].position = position2;
								list2[k + 1][0].position = position2;
							}
						}
						UIVertex[] verts = new UIVertex[]
						{
							list2[k][2],
							list2[k][3],
							list2[k + 1][0],
							list2[k + 1][1]
						};
						vh.AddUIVertexQuad(verts);
					}
				}
				vh.AddUIVertexQuad(list2[k]);
			}
			if (vh.currentVertCount > 64000)
			{
				UnityEngine.Debug.LogError("Max Verticies size is 64000, current mesh vertcies count is [" + vh.currentVertCount + "] - Cannot Draw");
				vh.Clear();
				return;
			}
		}

		private UIVertex[] CreateLineCap(Vector2 start, Vector2 end, UILineRenderer.SegmentType type)
		{
			if (type == UILineRenderer.SegmentType.Start)
			{
				Vector2 start2 = start - (end - start).normalized * this.lineThickness / 2f;
				return this.CreateLineSegment(start2, start, UILineRenderer.SegmentType.Start);
			}
			if (type == UILineRenderer.SegmentType.End)
			{
				Vector2 end2 = end + (end - start).normalized * this.lineThickness / 2f;
				return this.CreateLineSegment(end, end2, UILineRenderer.SegmentType.End);
			}
			UnityEngine.Debug.LogError("Bad SegmentType passed in to CreateLineCap. Must be SegmentType.Start or SegmentType.End");
			return null;
		}

		private UIVertex[] CreateLineSegment(Vector2 start, Vector2 end, UILineRenderer.SegmentType type)
		{
			Vector2 vector = new Vector2(start.y - end.y, end.x - start.x);
			Vector2 b = vector.normalized * this.lineThickness / 2f;
			Vector2 vector2 = start - b;
			Vector2 vector3 = start + b;
			Vector2 vector4 = end + b;
			Vector2 vector5 = end - b;
			switch (type)
			{
			case UILineRenderer.SegmentType.Start:
				return base.SetVbo(new Vector2[]
				{
					vector2,
					vector3,
					vector4,
					vector5
				}, UILineRenderer.startUvs);
			case UILineRenderer.SegmentType.End:
				return base.SetVbo(new Vector2[]
				{
					vector2,
					vector3,
					vector4,
					vector5
				}, UILineRenderer.endUvs);
			case UILineRenderer.SegmentType.Full:
				return base.SetVbo(new Vector2[]
				{
					vector2,
					vector3,
					vector4,
					vector5
				}, UILineRenderer.fullUvs);
			}
			return base.SetVbo(new Vector2[]
			{
				vector2,
				vector3,
				vector4,
				vector5
			}, UILineRenderer.middleUvs);
		}

		protected override void GeneratedUVs()
		{
			if (base.activeSprite != null)
			{
				Vector4 outerUV = DataUtility.GetOuterUV(base.activeSprite);
				Vector4 innerUV = DataUtility.GetInnerUV(base.activeSprite);
				UILineRenderer.UV_TOP_LEFT = new Vector2(outerUV.x, outerUV.y);
				UILineRenderer.UV_BOTTOM_LEFT = new Vector2(outerUV.x, outerUV.w);
				UILineRenderer.UV_TOP_CENTER_LEFT = new Vector2(innerUV.x, innerUV.y);
				UILineRenderer.UV_TOP_CENTER_RIGHT = new Vector2(innerUV.z, innerUV.y);
				UILineRenderer.UV_BOTTOM_CENTER_LEFT = new Vector2(innerUV.x, innerUV.w);
				UILineRenderer.UV_BOTTOM_CENTER_RIGHT = new Vector2(innerUV.z, innerUV.w);
				UILineRenderer.UV_TOP_RIGHT = new Vector2(outerUV.z, outerUV.y);
				UILineRenderer.UV_BOTTOM_RIGHT = new Vector2(outerUV.z, outerUV.w);
			}
			else
			{
				UILineRenderer.UV_TOP_LEFT = Vector2.zero;
				UILineRenderer.UV_BOTTOM_LEFT = new Vector2(0f, 1f);
				UILineRenderer.UV_TOP_CENTER_LEFT = new Vector2(0.5f, 0f);
				UILineRenderer.UV_TOP_CENTER_RIGHT = new Vector2(0.5f, 0f);
				UILineRenderer.UV_BOTTOM_CENTER_LEFT = new Vector2(0.5f, 1f);
				UILineRenderer.UV_BOTTOM_CENTER_RIGHT = new Vector2(0.5f, 1f);
				UILineRenderer.UV_TOP_RIGHT = new Vector2(1f, 0f);
				UILineRenderer.UV_BOTTOM_RIGHT = Vector2.one;
			}
			UILineRenderer.startUvs = new Vector2[]
			{
				UILineRenderer.UV_TOP_LEFT,
				UILineRenderer.UV_BOTTOM_LEFT,
				UILineRenderer.UV_BOTTOM_CENTER_LEFT,
				UILineRenderer.UV_TOP_CENTER_LEFT
			};
			UILineRenderer.middleUvs = new Vector2[]
			{
				UILineRenderer.UV_TOP_CENTER_LEFT,
				UILineRenderer.UV_BOTTOM_CENTER_LEFT,
				UILineRenderer.UV_BOTTOM_CENTER_RIGHT,
				UILineRenderer.UV_TOP_CENTER_RIGHT
			};
			UILineRenderer.endUvs = new Vector2[]
			{
				UILineRenderer.UV_TOP_CENTER_RIGHT,
				UILineRenderer.UV_BOTTOM_CENTER_RIGHT,
				UILineRenderer.UV_BOTTOM_RIGHT,
				UILineRenderer.UV_TOP_RIGHT
			};
			UILineRenderer.fullUvs = new Vector2[]
			{
				UILineRenderer.UV_TOP_LEFT,
				UILineRenderer.UV_BOTTOM_LEFT,
				UILineRenderer.UV_BOTTOM_RIGHT,
				UILineRenderer.UV_TOP_RIGHT
			};
		}

		protected override void ResolutionToNativeSize(float distance)
		{
			if (base.UseNativeSize)
			{
				this.m_Resolution = distance / (base.activeSprite.rect.width / base.pixelsPerUnit);
				this.lineThickness = base.activeSprite.rect.height / base.pixelsPerUnit;
			}
		}

		private const float MIN_MITER_JOIN = 0.2617994f;

		private const float MIN_BEVEL_NICE_JOIN = 0.5235988f;

		private static Vector2 UV_TOP_LEFT;

		private static Vector2 UV_BOTTOM_LEFT;

		private static Vector2 UV_TOP_CENTER_LEFT;

		private static Vector2 UV_TOP_CENTER_RIGHT;

		private static Vector2 UV_BOTTOM_CENTER_LEFT;

		private static Vector2 UV_BOTTOM_CENTER_RIGHT;

		private static Vector2 UV_TOP_RIGHT;

		private static Vector2 UV_BOTTOM_RIGHT;

		private static Vector2[] startUvs;

		private static Vector2[] middleUvs;

		private static Vector2[] endUvs;

		private static Vector2[] fullUvs;

		[SerializeField]
		[Tooltip("Points to draw lines between\n Can be improved using the Resolution Option")]
		internal Vector2[] m_points;

		[SerializeField]
		[Tooltip("Thickness of the line")]
		internal float lineThickness = 2f;

		[SerializeField]
		[Tooltip("Use the relative bounds of the Rect Transform (0,0 -> 0,1) or screen space coordinates")]
		internal bool relativeSize;

		[SerializeField]
		[Tooltip("Do the points identify a single line or split pairs of lines")]
		internal bool lineList;

		[SerializeField]
		[Tooltip("Add end caps to each line\nMultiple caps when used with Line List")]
		internal bool lineCaps;

		[SerializeField]
		[Tooltip("Resolution of the Bezier curve, different to line Resolution")]
		internal int bezierSegmentsPerCurve = 10;

		[Tooltip("The type of Join used between lines, Square/Mitre or Curved/Bevel")]
		public UILineRenderer.JoinType LineJoins;

		[Tooltip("Bezier method to apply to line, see docs for options\nCan't be used in conjunction with Resolution as Bezier already changes the resolution")]
		public UILineRenderer.BezierType BezierMode;

		[HideInInspector]
		public bool drivenExternally;

		private enum SegmentType
		{
			Start,
			Middle,
			End,
			Full
		}

		public enum JoinType
		{
			Bevel,
			Miter
		}

		public enum BezierType
		{
			None,
			Quick,
			Basic,
			Improved,
			Catenary
		}
	}
}
