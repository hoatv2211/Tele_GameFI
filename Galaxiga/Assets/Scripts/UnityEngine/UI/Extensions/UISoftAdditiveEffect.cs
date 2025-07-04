using System;

namespace UnityEngine.UI.Extensions
{
	[AddComponentMenu("UI/Effects/Extensions/UISoftAdditiveEffect")]
	[ExecuteInEditMode]
	[RequireComponent(typeof(RectTransform))]
	public class UISoftAdditiveEffect : MonoBehaviour
	{
		private void Start()
		{
			this.SetMaterial();
		}

		public void SetMaterial()
		{
			this.mGraphic = base.GetComponent<MaskableGraphic>();
			if (this.mGraphic != null)
			{
				if (this.mGraphic.material == null || this.mGraphic.material.name == "Default UI Material")
				{
					this.mGraphic.material = new Material(Shader.Find("UI Extensions/UISoftAdditive"));
				}
			}
			else
			{
				UnityEngine.Debug.LogError("Please attach component to a Graphical UI component");
			}
		}

		public void OnValidate()
		{
			this.SetMaterial();
		}

		private MaskableGraphic mGraphic;
	}
}
