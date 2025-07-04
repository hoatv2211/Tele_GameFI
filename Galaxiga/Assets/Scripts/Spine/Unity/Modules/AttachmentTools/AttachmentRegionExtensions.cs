using System;
using UnityEngine;

namespace Spine.Unity.Modules.AttachmentTools
{
	public static class AttachmentRegionExtensions
	{
		public static AtlasRegion GetRegion(this Attachment attachment)
		{
			IHasRendererObject hasRendererObject = attachment as IHasRendererObject;
			if (hasRendererObject != null)
			{
				return hasRendererObject.RendererObject as AtlasRegion;
			}
			return null;
		}

		public static AtlasRegion GetRegion(this RegionAttachment regionAttachment)
		{
			return regionAttachment.RendererObject as AtlasRegion;
		}

		public static AtlasRegion GetRegion(this MeshAttachment meshAttachment)
		{
			return meshAttachment.RendererObject as AtlasRegion;
		}

		public static void SetRegion(this Attachment attachment, AtlasRegion region, bool updateOffset = true)
		{
			RegionAttachment regionAttachment = attachment as RegionAttachment;
			if (regionAttachment != null)
			{
				regionAttachment.SetRegion(region, updateOffset);
			}
			MeshAttachment meshAttachment = attachment as MeshAttachment;
			if (meshAttachment != null)
			{
				meshAttachment.SetRegion(region, updateOffset);
			}
		}

		public static void SetRegion(this RegionAttachment attachment, AtlasRegion region, bool updateOffset = true)
		{
			if (region == null)
			{
				throw new ArgumentNullException("region");
			}
			attachment.RendererObject = region;
			attachment.SetUVs(region.u, region.v, region.u2, region.v2, region.rotate);
			attachment.regionOffsetX = region.offsetX;
			attachment.regionOffsetY = region.offsetY;
			attachment.regionWidth = (float)region.width;
			attachment.regionHeight = (float)region.height;
			attachment.regionOriginalWidth = (float)region.originalWidth;
			attachment.regionOriginalHeight = (float)region.originalHeight;
			if (updateOffset)
			{
				attachment.UpdateOffset();
			}
		}

		public static void SetRegion(this MeshAttachment attachment, AtlasRegion region, bool updateUVs = true)
		{
			if (region == null)
			{
				throw new ArgumentNullException("region");
			}
			attachment.RendererObject = region;
			attachment.RegionU = region.u;
			attachment.RegionV = region.v;
			attachment.RegionU2 = region.u2;
			attachment.RegionV2 = region.v2;
			attachment.RegionRotate = region.rotate;
			attachment.regionOffsetX = region.offsetX;
			attachment.regionOffsetY = region.offsetY;
			attachment.regionWidth = (float)region.width;
			attachment.regionHeight = (float)region.height;
			attachment.regionOriginalWidth = (float)region.originalWidth;
			attachment.regionOriginalHeight = (float)region.originalHeight;
			if (updateUVs)
			{
				attachment.UpdateUVs();
			}
		}

		public static RegionAttachment ToRegionAttachment(this Sprite sprite, Material material, float rotation = 0f)
		{
			return sprite.ToRegionAttachment(material.ToSpineAtlasPage(), rotation);
		}

		public static RegionAttachment ToRegionAttachment(this Sprite sprite, AtlasPage page, float rotation = 0f)
		{
			if (sprite == null)
			{
				throw new ArgumentNullException("sprite");
			}
			if (page == null)
			{
				throw new ArgumentNullException("page");
			}
			AtlasRegion region = sprite.ToAtlasRegion(page);
			float scale = 1f / sprite.pixelsPerUnit;
			return region.ToRegionAttachment(sprite.name, scale, rotation);
		}

		public static RegionAttachment ToRegionAttachmentPMAClone(this Sprite sprite, Shader shader, TextureFormat textureFormat = TextureFormat.RGBA32, bool mipmaps = false, Material materialPropertySource = null, float rotation = 0f)
		{
			if (sprite == null)
			{
				throw new ArgumentNullException("sprite");
			}
			if (shader == null)
			{
				throw new ArgumentNullException("shader");
			}
			AtlasRegion region = sprite.ToAtlasRegionPMAClone(shader, textureFormat, mipmaps, materialPropertySource);
			float scale = 1f / sprite.pixelsPerUnit;
			return region.ToRegionAttachment(sprite.name, scale, rotation);
		}

		public static RegionAttachment ToRegionAttachmentPMAClone(this Sprite sprite, Material materialPropertySource, TextureFormat textureFormat = TextureFormat.RGBA32, bool mipmaps = false, float rotation = 0f)
		{
			return sprite.ToRegionAttachmentPMAClone(materialPropertySource.shader, textureFormat, mipmaps, materialPropertySource, rotation);
		}

		public static RegionAttachment ToRegionAttachment(this AtlasRegion region, string attachmentName, float scale = 0.01f, float rotation = 0f)
		{
			if (string.IsNullOrEmpty(attachmentName))
			{
				throw new ArgumentException("attachmentName can't be null or empty.", "attachmentName");
			}
			if (region == null)
			{
				throw new ArgumentNullException("region");
			}
			RegionAttachment regionAttachment = new RegionAttachment(attachmentName);
			regionAttachment.RendererObject = region;
			regionAttachment.SetUVs(region.u, region.v, region.u2, region.v2, region.rotate);
			regionAttachment.regionOffsetX = region.offsetX;
			regionAttachment.regionOffsetY = region.offsetY;
			regionAttachment.regionWidth = (float)region.width;
			regionAttachment.regionHeight = (float)region.height;
			regionAttachment.regionOriginalWidth = (float)region.originalWidth;
			regionAttachment.regionOriginalHeight = (float)region.originalHeight;
			regionAttachment.Path = region.name;
			regionAttachment.scaleX = 1f;
			regionAttachment.scaleY = 1f;
			regionAttachment.rotation = rotation;
			regionAttachment.r = 1f;
			regionAttachment.g = 1f;
			regionAttachment.b = 1f;
			regionAttachment.a = 1f;
			regionAttachment.width = regionAttachment.regionOriginalWidth * scale;
			regionAttachment.height = regionAttachment.regionOriginalHeight * scale;
			regionAttachment.SetColor(Color.white);
			regionAttachment.UpdateOffset();
			return regionAttachment;
		}

		public static void SetScale(this RegionAttachment regionAttachment, Vector2 scale)
		{
			regionAttachment.scaleX = scale.x;
			regionAttachment.scaleY = scale.y;
		}

		public static void SetScale(this RegionAttachment regionAttachment, float x, float y)
		{
			regionAttachment.scaleX = x;
			regionAttachment.scaleY = y;
		}

		public static void SetPositionOffset(this RegionAttachment regionAttachment, Vector2 offset)
		{
			regionAttachment.x = offset.x;
			regionAttachment.y = offset.y;
		}

		public static void SetPositionOffset(this RegionAttachment regionAttachment, float x, float y)
		{
			regionAttachment.x = x;
			regionAttachment.y = y;
		}

		public static void SetRotation(this RegionAttachment regionAttachment, float rotation)
		{
			regionAttachment.rotation = rotation;
		}
	}
}
