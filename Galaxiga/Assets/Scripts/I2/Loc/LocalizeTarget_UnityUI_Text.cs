using System;
using UnityEngine;
using UnityEngine.UI;

namespace I2.Loc
{
	public class LocalizeTarget_UnityUI_Text : LocalizeTarget<Text>
	{
		static LocalizeTarget_UnityUI_Text()
		{
			LocalizeTarget_UnityUI_Text.AutoRegister();
		}

		[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
		private static void AutoRegister()
		{
			LocalizationManager.RegisterTarget(new LocalizeTargetDesc_Type<Text, LocalizeTarget_UnityUI_Text>
			{
				Name = "Text",
				Priority = 100
			});
		}

		public override eTermType GetPrimaryTermType(Localize cmp)
		{
			return eTermType.Text;
		}

		public override eTermType GetSecondaryTermType(Localize cmp)
		{
			return eTermType.Font;
		}

		public override bool CanUseSecondaryTerm()
		{
			return true;
		}

		public override bool AllowMainTermToBeRTL()
		{
			return true;
		}

		public override bool AllowSecondTermToBeRTL()
		{
			return false;
		}

		public override void GetFinalTerms(Localize cmp, string Main, string Secondary, out string primaryTerm, out string secondaryTerm)
		{
			primaryTerm = ((!this.mTarget) ? null : this.mTarget.text);
			secondaryTerm = ((!(this.mTarget.font != null)) ? string.Empty : this.mTarget.font.name);
		}

		public override void DoLocalize(Localize cmp, string mainTranslation, string secondaryTranslation)
		{
			Font secondaryTranslatedObj = cmp.GetSecondaryTranslatedObj<Font>(ref mainTranslation, ref secondaryTranslation);
			if (secondaryTranslatedObj != null && secondaryTranslatedObj != this.mTarget.font)
			{
				this.mTarget.font = secondaryTranslatedObj;
			}
			if (this.mInitializeAlignment)
			{
				this.mInitializeAlignment = false;
				this.mAlignmentWasRTL = LocalizationManager.IsRight2Left;
				this.InitAlignment(this.mAlignmentWasRTL, this.mTarget.alignment, out this.mAlignment_LTR, out this.mAlignment_RTL);
			}
			else
			{
				TextAnchor textAnchor;
				TextAnchor textAnchor2;
				this.InitAlignment(this.mAlignmentWasRTL, this.mTarget.alignment, out textAnchor, out textAnchor2);
				if ((this.mAlignmentWasRTL && this.mAlignment_RTL != textAnchor2) || (!this.mAlignmentWasRTL && this.mAlignment_LTR != textAnchor))
				{
					this.mAlignment_LTR = textAnchor;
					this.mAlignment_RTL = textAnchor2;
				}
				this.mAlignmentWasRTL = LocalizationManager.IsRight2Left;
			}
			if (mainTranslation != null && this.mTarget.text != mainTranslation)
			{
				if (cmp.CorrectAlignmentForRTL)
				{
					this.mTarget.alignment = ((!LocalizationManager.IsRight2Left) ? this.mAlignment_LTR : this.mAlignment_RTL);
				}
				this.mTarget.text = mainTranslation;
				this.mTarget.SetVerticesDirty();
			}
		}

		private void InitAlignment(bool isRTL, TextAnchor alignment, out TextAnchor alignLTR, out TextAnchor alignRTL)
		{
			alignRTL = alignment;
			alignLTR = alignment;
			if (isRTL)
			{
				switch (alignment)
				{
				case TextAnchor.UpperLeft:
					alignLTR = TextAnchor.UpperRight;
					break;
				case TextAnchor.UpperRight:
					alignLTR = TextAnchor.UpperLeft;
					break;
				case TextAnchor.MiddleLeft:
					alignLTR = TextAnchor.MiddleRight;
					break;
				case TextAnchor.MiddleRight:
					alignLTR = TextAnchor.MiddleLeft;
					break;
				case TextAnchor.LowerLeft:
					alignLTR = TextAnchor.LowerRight;
					break;
				case TextAnchor.LowerRight:
					alignLTR = TextAnchor.LowerLeft;
					break;
				}
			}
			else
			{
				switch (alignment)
				{
				case TextAnchor.UpperLeft:
					alignRTL = TextAnchor.UpperRight;
					break;
				case TextAnchor.UpperRight:
					alignRTL = TextAnchor.UpperLeft;
					break;
				case TextAnchor.MiddleLeft:
					alignRTL = TextAnchor.MiddleRight;
					break;
				case TextAnchor.MiddleRight:
					alignRTL = TextAnchor.MiddleLeft;
					break;
				case TextAnchor.LowerLeft:
					alignRTL = TextAnchor.LowerRight;
					break;
				case TextAnchor.LowerRight:
					alignRTL = TextAnchor.LowerLeft;
					break;
				}
			}
		}

		private TextAnchor mAlignment_RTL = TextAnchor.UpperRight;

		private TextAnchor mAlignment_LTR;

		private bool mAlignmentWasRTL;

		private bool mInitializeAlignment = true;
	}
}
