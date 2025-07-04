using System;

namespace Spine
{
	public class ColorTimeline : CurveTimeline
	{
		public ColorTimeline(int frameCount) : base(frameCount)
		{
			this.frames = new float[frameCount * 5];
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

		public override int PropertyId
		{
			get
			{
				return 83886080 + this.slotIndex;
			}
		}

		public void SetFrame(int frameIndex, float time, float r, float g, float b, float a)
		{
			frameIndex *= 5;
			this.frames[frameIndex] = time;
			this.frames[frameIndex + 1] = r;
			this.frames[frameIndex + 2] = g;
			this.frames[frameIndex + 3] = b;
			this.frames[frameIndex + 4] = a;
		}

		public override void Apply(Skeleton skeleton, float lastTime, float time, ExposedList<Event> firedEvents, float alpha, MixPose pose, MixDirection direction)
		{
			Slot slot = skeleton.slots.Items[this.slotIndex];
			float[] array = this.frames;
			if (time >= array[0])
			{
				float num2;
				float num3;
				float num4;
				float num5;
				if (time >= array[array.Length - 5])
				{
					int num = array.Length;
					num2 = array[num + -4];
					num3 = array[num + -3];
					num4 = array[num + -2];
					num5 = array[num + -1];
				}
				else
				{
					int num6 = Animation.BinarySearch(array, time, 5);
					num2 = array[num6 + -4];
					num3 = array[num6 + -3];
					num4 = array[num6 + -2];
					num5 = array[num6 + -1];
					float num7 = array[num6];
					float curvePercent = base.GetCurvePercent(num6 / 5 - 1, 1f - (time - num7) / (array[num6 + -5] - num7));
					num2 += (array[num6 + 1] - num2) * curvePercent;
					num3 += (array[num6 + 2] - num3) * curvePercent;
					num4 += (array[num6 + 3] - num4) * curvePercent;
					num5 += (array[num6 + 4] - num5) * curvePercent;
				}
				if (alpha == 1f)
				{
					slot.r = num2;
					slot.g = num3;
					slot.b = num4;
					slot.a = num5;
				}
				else
				{
					float r;
					float g;
					float b;
					float a;
					if (pose == MixPose.Setup)
					{
						r = slot.data.r;
						g = slot.data.g;
						b = slot.data.b;
						a = slot.data.a;
					}
					else
					{
						r = slot.r;
						g = slot.g;
						b = slot.b;
						a = slot.a;
					}
					slot.r = r + (num2 - r) * alpha;
					slot.g = g + (num3 - g) * alpha;
					slot.b = b + (num4 - b) * alpha;
					slot.a = a + (num5 - a) * alpha;
				}
				return;
			}
			SlotData data = slot.data;
			if (pose == MixPose.Setup)
			{
				slot.r = data.r;
				slot.g = data.g;
				slot.b = data.b;
				slot.a = data.a;
				return;
			}
			if (pose != MixPose.Current)
			{
				return;
			}
			slot.r += (slot.r - data.r) * alpha;
			slot.g += (slot.g - data.g) * alpha;
			slot.b += (slot.b - data.b) * alpha;
			slot.a += (slot.a - data.a) * alpha;
		}

		public const int ENTRIES = 5;

		protected const int PREV_TIME = -5;

		protected const int PREV_R = -4;

		protected const int PREV_G = -3;

		protected const int PREV_B = -2;

		protected const int PREV_A = -1;

		protected const int R = 1;

		protected const int G = 2;

		protected const int B = 3;

		protected const int A = 4;

		internal int slotIndex;

		internal float[] frames;
	}
}
