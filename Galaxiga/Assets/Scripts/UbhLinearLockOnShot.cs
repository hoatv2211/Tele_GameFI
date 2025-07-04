using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Serialization;

[AddComponentMenu("UniBulletHell/Shot Pattern/Linear Shot (Lock On)")]
public class UbhLinearLockOnShot : UbhLinearShot
{
	public override bool lockOnShot
	{
		get
		{
			return true;
		}
	}

	public override void Shot()
	{
		if (PlaneIngameManager.current.CurrentTransformPlayer != null)
		{
			this.m_targetTransform = PlaneIngameManager.current.CurrentTransformPlayer;
		}
		if (this.m_shooting)
		{
			return;
		}
		this.AimTarget();
		if (this.m_targetTransform == null)
		{
			UnityEngine.Debug.LogWarning("Cannot shot because TargetTransform is not set.");
			return;
		}
		base.Shot();
		if (this.m_aiming)
		{
			base.StartCoroutine(this.AimingCoroutine());
		}
	}

	private void AimTarget()
	{
		if (this.m_targetTransform == null && this.m_setTargetFromTag)
		{
			this.m_targetTransform = UbhUtil.GetTransformFromTagName(this.m_targetTagName, this.m_randomSelectTagTarget);
		}
		this.m_angle = UbhUtil.GetAngleFromTwoPosition(base.transform, this.m_targetTransform, base.shotCtrl.m_axisMove);
	}

	private IEnumerator AimingCoroutine()
	{
		if (this.m_targetTransform != null && PlaneIngameManager.current.CurrentTransformPlayer != null)
		{
			this.m_targetTransform = PlaneIngameManager.current.CurrentTransformPlayer;
		}
		while (this.m_aiming)
		{
			if (!this.m_shooting)
			{
				yield break;
			}
			this.AimTarget();
			yield return null;
		}
		yield break;
	}

	[Header("===== LinearLockOnShot Settings =====")]
	[FormerlySerializedAs("_SetTargetFromTag")]
	public bool m_setTargetFromTag = true;

	[FormerlySerializedAs("_TargetTagName")]
	[UbhConditionalHide("m_setTargetFromTag")]
	public string m_targetTagName = "Player";

	public bool m_randomSelectTagTarget;

	[FormerlySerializedAs("_TargetTransform")]
	public Transform m_targetTransform;

	[FormerlySerializedAs("_Aiming")]
	public bool m_aiming;
}
