using System;
using System.Collections.Generic;

namespace Spine
{
	public static class SkeletonExtensions
	{
		public static bool IsWeighted(this VertexAttachment va)
		{
			return va.bones != null && va.bones.Length > 0;
		}

		public static bool IsRenderable(this Attachment a)
		{
			return a is IHasRendererObject;
		}

		public static bool InheritsRotation(this TransformMode mode)
		{
			return ((long)mode & 1L) == 0L;
		}

		public static bool InheritsScale(this TransformMode mode)
		{
			return ((long)mode & 2L) == 0L;
		}

		internal static void SetPropertyToSetupPose(this Skeleton skeleton, int propertyID)
		{
			int num = propertyID >> 24;
			TimelineType timelineType = (TimelineType)num;
			int num2 = propertyID - (num << 24);
			switch (timelineType)
			{
			case TimelineType.Rotate:
			{
				Bone bone = skeleton.bones.Items[num2];
				bone.rotation = bone.data.rotation;
				break;
			}
			case TimelineType.Translate:
			{
				Bone bone = skeleton.bones.Items[num2];
				bone.x = bone.data.x;
				bone.y = bone.data.y;
				break;
			}
			case TimelineType.Scale:
			{
				Bone bone = skeleton.bones.Items[num2];
				bone.scaleX = bone.data.scaleX;
				bone.scaleY = bone.data.scaleY;
				break;
			}
			case TimelineType.Shear:
			{
				Bone bone = skeleton.bones.Items[num2];
				bone.shearX = bone.data.shearX;
				bone.shearY = bone.data.shearY;
				break;
			}
			case TimelineType.Attachment:
				skeleton.SetSlotAttachmentToSetupPose(num2);
				break;
			case TimelineType.Color:
				skeleton.slots.Items[num2].SetColorToSetupPose();
				break;
			case TimelineType.Deform:
				skeleton.slots.Items[num2].attachmentVertices.Clear(true);
				break;
			case TimelineType.DrawOrder:
				skeleton.SetDrawOrderToSetupPose();
				break;
			case TimelineType.IkConstraint:
			{
				IkConstraint ikConstraint = skeleton.ikConstraints.Items[num2];
				ikConstraint.mix = ikConstraint.data.mix;
				ikConstraint.bendDirection = ikConstraint.data.bendDirection;
				break;
			}
			case TimelineType.TransformConstraint:
			{
				TransformConstraint transformConstraint = skeleton.transformConstraints.Items[num2];
				TransformConstraintData data = transformConstraint.data;
				transformConstraint.rotateMix = data.rotateMix;
				transformConstraint.translateMix = data.translateMix;
				transformConstraint.scaleMix = data.scaleMix;
				transformConstraint.shearMix = data.shearMix;
				break;
			}
			case TimelineType.PathConstraintPosition:
			{
				PathConstraint pathConstraint = skeleton.pathConstraints.Items[num2];
				pathConstraint.position = pathConstraint.data.position;
				break;
			}
			case TimelineType.PathConstraintSpacing:
			{
				PathConstraint pathConstraint = skeleton.pathConstraints.Items[num2];
				pathConstraint.spacing = pathConstraint.data.spacing;
				break;
			}
			case TimelineType.PathConstraintMix:
			{
				PathConstraint pathConstraint = skeleton.pathConstraints.Items[num2];
				pathConstraint.rotateMix = pathConstraint.data.rotateMix;
				pathConstraint.translateMix = pathConstraint.data.translateMix;
				break;
			}
			case TimelineType.TwoColor:
				skeleton.slots.Items[num2].SetColorToSetupPose();
				break;
			}
		}

		public static void SetDrawOrderToSetupPose(this Skeleton skeleton)
		{
			Slot[] items = skeleton.slots.Items;
			int count = skeleton.slots.Count;
			ExposedList<Slot> drawOrder = skeleton.drawOrder;
			drawOrder.Clear(false);
			drawOrder.GrowIfNeeded(count);
			Array.Copy(items, drawOrder.Items, count);
		}

		public static void SetColorToSetupPose(this Slot slot)
		{
			slot.r = slot.data.r;
			slot.g = slot.data.g;
			slot.b = slot.data.b;
			slot.a = slot.data.a;
			slot.r2 = slot.data.r2;
			slot.g2 = slot.data.g2;
			slot.b2 = slot.data.b2;
		}

		public static void SetAttachmentToSetupPose(this Slot slot)
		{
			SlotData data = slot.data;
			slot.Attachment = slot.bone.skeleton.GetAttachment(data.name, data.attachmentName);
		}

		public static void SetSlotAttachmentToSetupPose(this Skeleton skeleton, int slotIndex)
		{
			Slot slot = skeleton.slots.Items[slotIndex];
			string attachmentName = slot.data.attachmentName;
			if (string.IsNullOrEmpty(attachmentName))
			{
				slot.Attachment = null;
			}
			else
			{
				Attachment attachment = skeleton.GetAttachment(slotIndex, attachmentName);
				slot.Attachment = attachment;
			}
		}

		public static void PoseWithAnimation(this Skeleton skeleton, string animationName, float time, bool loop = false)
		{
			Animation animation = skeleton.data.FindAnimation(animationName);
			if (animation == null)
			{
				return;
			}
			animation.Apply(skeleton, 0f, time, loop, null, 1f, MixPose.Setup, MixDirection.In);
		}

		public static void PoseSkeleton(this Animation animation, Skeleton skeleton, float time, bool loop = false)
		{
			animation.Apply(skeleton, 0f, time, loop, null, 1f, MixPose.Setup, MixDirection.In);
		}

		public static void SetKeyedItemsToSetupPose(this Animation animation, Skeleton skeleton)
		{
			animation.Apply(skeleton, 0f, 0f, false, null, 0f, MixPose.Setup, MixDirection.Out);
		}

		public static void FindNamesForSlot(this Skin skin, string slotName, SkeletonData skeletonData, List<string> results)
		{
			int slotIndex = skeletonData.FindSlotIndex(slotName);
			skin.FindNamesForSlot(slotIndex, results);
		}

		public static void FindAttachmentsForSlot(this Skin skin, string slotName, SkeletonData skeletonData, List<Attachment> results)
		{
			int slotIndex = skeletonData.FindSlotIndex(slotName);
			skin.FindAttachmentsForSlot(slotIndex, results);
		}
	}
}
