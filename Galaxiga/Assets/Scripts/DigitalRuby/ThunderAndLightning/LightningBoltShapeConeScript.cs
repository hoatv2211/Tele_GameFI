using System;
using UnityEngine;

namespace DigitalRuby.ThunderAndLightning
{
	public class LightningBoltShapeConeScript : LightningBoltPrefabScriptBase
	{
		public override void CreateLightningBolt(LightningBoltParameters parameters)
		{
			Vector2 vector = UnityEngine.Random.insideUnitCircle * this.InnerRadius;
			Vector3 start = base.transform.rotation * new Vector3(vector.x, vector.y, 0f);
			Vector2 vector2 = UnityEngine.Random.insideUnitCircle * this.OuterRadius;
			Vector3 end = base.transform.rotation * new Vector3(vector2.x, vector2.y, 0f) + base.transform.forward * this.Length;
			parameters.Start = start;
			parameters.End = end;
			base.CreateLightningBolt(parameters);
		}

		[Header("Lightning Cone Properties")]
		[Tooltip("Radius at base of cone where lightning can emit from")]
		public float InnerRadius = 0.1f;

		[Tooltip("Radius at outer part of the cone where lightning emits to")]
		public float OuterRadius = 4f;

		[Tooltip("The length of the cone from the center of the inner and outer circle")]
		public float Length = 4f;
	}
}
