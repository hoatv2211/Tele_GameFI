using System;

namespace Spine
{
	public class DeformTimeline : CurveTimeline
	{
		public DeformTimeline(int frameCount) : base(frameCount)
		{
			this.frames = new float[frameCount];
			this.frameVertices = new float[frameCount][];
		}

		public int SlotIndex
		{
			get
			{
				return this.slotIndex;
			}
			set
			{
				this.slotIndex = value;
			}
		}

		public float[] Frames
		{
			get
			{
				return this.frames;
			}
			set
			{
				this.frames = value;
			}
		}

		public float[][] Vertices
		{
			get
			{
				return this.frameVertices;
			}
			set
			{
				this.frameVertices = value;
			}
		}

		public VertexAttachment Attachment
		{
			get
			{
				return this.attachment;
			}
			set
			{
				this.attachment = value;
			}
		}

		public override int PropertyId
		{
			get
			{
				return 100663296 + this.attachment.id + this.slotIndex;
			}
		}

		public void SetFrame(int frameIndex, float time, float[] vertices)
		{
			this.frames[frameIndex] = time;
			this.frameVertices[frameIndex] = vertices;
		}

		public override void Apply(Skeleton skeleton, float lastTime, float time, ExposedList<Event> firedEvents, float alpha, MixPose pose, MixDirection direction)
		{
			Slot slot = skeleton.slots.Items[this.slotIndex];
			VertexAttachment vertexAttachment = slot.attachment as VertexAttachment;
			if (vertexAttachment == null || !vertexAttachment.ApplyDeform(this.attachment))
			{
				return;
			}
			ExposedList<float> attachmentVertices = slot.attachmentVertices;
			if (attachmentVertices.Count == 0)
			{
				alpha = 1f;
			}
			float[][] array = this.frameVertices;
			int num = array[0].Length;
			float[] array2 = this.frames;
			if (time < array2[0])
			{
				if (pose == MixPose.Setup)
				{
					attachmentVertices.Clear(true);
					return;
				}
				if (pose != MixPose.Current)
				{
					return;
				}
				if (alpha == 1f)
				{
					attachmentVertices.Clear(true);
					return;
				}
				if (attachmentVertices.Capacity < num)
				{
					attachmentVertices.Capacity = num;
				}
				attachmentVertices.Count = num;
				float[] items = attachmentVertices.Items;
				if (vertexAttachment.bones == null)
				{
					float[] vertices = vertexAttachment.vertices;
					for (int i = 0; i < num; i++)
					{
						items[i] += (vertices[i] - items[i]) * alpha;
					}
				}
				else
				{
					alpha = 1f - alpha;
					for (int j = 0; j < num; j++)
					{
						items[j] *= alpha;
					}
				}
				return;
			}
			else
			{
				if (attachmentVertices.Capacity < num)
				{
					attachmentVertices.Capacity = num;
				}
				attachmentVertices.Count = num;
				float[] items = attachmentVertices.Items;
				if (time >= array2[array2.Length - 1])
				{
					float[] array3 = array[array2.Length - 1];
					if (alpha == 1f)
					{
						Array.Copy(array3, 0, items, 0, num);
					}
					else if (pose == MixPose.Setup)
					{
						if (vertexAttachment.bones == null)
						{
							float[] vertices2 = vertexAttachment.vertices;
							for (int k = 0; k < num; k++)
							{
								float num2 = vertices2[k];
								items[k] = num2 + (array3[k] - num2) * alpha;
							}
						}
						else
						{
							for (int l = 0; l < num; l++)
							{
								items[l] = array3[l] * alpha;
							}
						}
					}
					else
					{
						for (int m = 0; m < num; m++)
						{
							items[m] += (array3[m] - items[m]) * alpha;
						}
					}
					return;
				}
				int num3 = Animation.BinarySearch(array2, time);
				float[] array4 = array[num3 - 1];
				float[] array5 = array[num3];
				float num4 = array2[num3];
				float curvePercent = base.GetCurvePercent(num3 - 1, 1f - (time - num4) / (array2[num3 - 1] - num4));
				if (alpha == 1f)
				{
					for (int n = 0; n < num; n++)
					{
						float num5 = array4[n];
						items[n] = num5 + (array5[n] - num5) * curvePercent;
					}
				}
				else if (pose == MixPose.Setup)
				{
					if (vertexAttachment.bones == null)
					{
						float[] vertices3 = vertexAttachment.vertices;
						for (int num6 = 0; num6 < num; num6++)
						{
							float num7 = array4[num6];
							float num8 = vertices3[num6];
							items[num6] = num8 + (num7 + (array5[num6] - num7) * curvePercent - num8) * alpha;
						}
					}
					else
					{
						for (int num9 = 0; num9 < num; num9++)
						{
							float num10 = array4[num9];
							items[num9] = (num10 + (array5[num9] - num10) * curvePercent) * alpha;
						}
					}
				}
				else
				{
					for (int num11 = 0; num11 < num; num11++)
					{
						float num12 = array4[num11];
						items[num11] += (num12 + (array5[num11] - num12) * curvePercent - items[num11]) * alpha;
					}
				}
				return;
			}
		}

		internal int slotIndex;

		internal float[] frames;

		internal float[][] frameVertices;

		internal VertexAttachment attachment;
	}
}
