using System;

namespace Spine
{
	public class RotateTimeline : CurveTimeline
	{
		public RotateTimeline(int frameCount) : base(frameCount)
		{
			this.frames = new float[frameCount << 1];
		}

		public int BoneIndex
		{
			get
			{
				return this.boneIndex;
			}
			set
			{
				this.boneIndex = value;
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
				return this.boneIndex;
			}
		}

		public void SetFrame(int frameIndex, float time, float degrees)
		{
			frameIndex <<= 1;
			this.frames[frameIndex] = time;
			this.frames[frameIndex + 1] = degrees;
		}

		public override void Apply(Skeleton skeleton, float lastTime, float time, ExposedList<Event> firedEvents, float alpha, MixPose pose, MixDirection direction)
		{
			Bone bone = skeleton.bones.Items[this.boneIndex];
			float[] array = this.frames;
			if (time < array[0])
			{
				if (pose == MixPose.Setup)
				{
					bone.rotation = bone.data.rotation;
					return;
				}
				if (pose != MixPose.Current)
				{
					return;
				}
				float num = bone.data.rotation - bone.rotation;
				num -= (float)((16384 - (int)(16384.499999999996 - (double)(num / 360f))) * 360);
				bone.rotation += num * alpha;
				return;
			}
			else
			{
				if (time >= array[array.Length - 2])
				{
					if (pose == MixPose.Setup)
					{
						bone.rotation = bone.data.rotation + array[array.Length + -1] * alpha;
					}
					else
					{
						float num2 = bone.data.rotation + array[array.Length + -1] - bone.rotation;
						num2 -= (float)((16384 - (int)(16384.499999999996 - (double)(num2 / 360f))) * 360);
						bone.rotation += num2 * alpha;
					}
					return;
				}
				int num3 = Animation.BinarySearch(array, time, 2);
				float num4 = array[num3 + -1];
				float num5 = array[num3];
				float curvePercent = base.GetCurvePercent((num3 >> 1) - 1, 1f - (time - num5) / (array[num3 + -2] - num5));
				float num6 = array[num3 + 1] - num4;
				num6 -= (float)((16384 - (int)(16384.499999999996 - (double)(num6 / 360f))) * 360);
				num6 = num4 + num6 * curvePercent;
				if (pose == MixPose.Setup)
				{
					num6 -= (float)((16384 - (int)(16384.499999999996 - (double)(num6 / 360f))) * 360);
					bone.rotation = bone.data.rotation + num6 * alpha;
				}
				else
				{
					num6 = bone.data.rotation + num6 - bone.rotation;
					num6 -= (float)((16384 - (int)(16384.499999999996 - (double)(num6 / 360f))) * 360);
					bone.rotation += num6 * alpha;
				}
				return;
			}
		}

		public const int ENTRIES = 2;

		internal const int PREV_TIME = -2;

		internal const int PREV_ROTATION = -1;

		internal const int ROTATION = 1;

		internal int boneIndex;

		internal float[] frames;
	}
}
