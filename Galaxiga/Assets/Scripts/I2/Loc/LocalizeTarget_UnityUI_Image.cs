using System;
using UnityEngine;
using UnityEngine.UI;

namespace I2.Loc
{
	public class LocalizeTarget_UnityUI_Image : LocalizeTarget<Image>
	{
		static LocalizeTarget_UnityUI_Image()
		{
			LocalizeTarget_UnityUI_Image.AutoRegister();
		}

		[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
		private static void AutoRegister()
		{
			LocalizationManager.RegisterTarget(new LocalizeTargetDesc_Type<Image, LocalizeTarget_UnityUI_Image>
			{
				Name = "Image",
				Priority = 100
			});
		}

		public override bool CanUseSecondaryTerm()
		{
			return false;
		}

		public override bool AllowMainTermToBeRTL()
		{
			return false;
		}

		public override bool AllowSecondTermToBeRTL()
		{
			return false;
		}

		public override eTermType GetPrimaryTermType(Localize cmp)
		{
			return (!(this.mTarget.sprite == null)) ? eTermType.Sprite : eTermType.Texture;
		}

		public override eTermType GetSecondaryTermType(Localize cmp)
		{
			return eTermType.Text;
		}

		public override void GetFinalTerms(Localize cmp, string Main, string Secondary, out string primaryTerm, out string secondaryTerm)
		{
			primaryTerm = ((!this.mTarget.mainTexture) ? string.Empty : this.mTarget.mainTexture.name);
			if (this.mTarget.sprite != null && this.mTarget.sprite.name != primaryTerm)
			{
				primaryTerm = primaryTerm + "." + this.mTarget.sprite.name;
			}
			secondaryTerm = null;
		}

		public override void DoLocalize(Localize cmp, string mainTranslation, string secondaryTranslation)
		{
			Sprite sprite = this.mTarget.sprite;
			if (sprite == null || sprite.name != mainTranslation)
			{
				this.mTarget.sprite = cmp.FindTranslatedObject<Sprite>(mainTranslation);
			}
		}
	}
}
