using System;
using UnityEngine;

public class GeneralAdvancedTechniques : MonoBehaviour
{
	private void Start()
	{
		LeanTween.alpha(this.avatarRecursive, 0f, 1f).setRecursive(true).setLoopPingPong();
		LeanTween.alpha(this.avatar2dRecursive, 0f, 1f).setRecursive(true).setLoopPingPong();
		LeanTween.alpha(this.wingPersonPanel, 0f, 1f).setRecursive(true).setLoopPingPong();
		LeanTween.value(this.avatarMove, 0f, (float)this.movePts.Length - 1f, 5f).setOnUpdate(delegate(float val)
		{
			int num = (int)Mathf.Floor(val);
			int num2 = (num >= this.movePts.Length - 1) ? num : (num + 1);
			float d = val - (float)num;
			Vector3 a = this.movePts[num2].position - this.movePts[num].position;
			this.avatarMove.transform.position = this.movePts[num].position + a * d;
		}).setEase(LeanTweenType.easeInOutExpo).setLoopPingPong();
		for (int i = 0; i < this.movePts.Length; i++)
		{
			LeanTween.moveY(this.movePts[i].gameObject, this.movePts[i].position.y + 1.5f, 1f).setDelay((float)i * 0.2f).setLoopPingPong();
		}
		for (int j = 0; j < this.avatarSpeed.Length; j++)
		{
			LeanTween.moveLocalZ(this.avatarSpeed[j], (float)(j + 1) * 5f, 1f).setSpeed(6f).setEase(LeanTweenType.easeInOutExpo).setLoopPingPong();
		}
		for (int k = 0; k < this.avatarSpeed2.Length; k++)
		{
			LeanTween.moveLocal(this.avatarSpeed2[k], (k != 0) ? this.circleLrg : this.circleSm, 1f).setSpeed(20f).setRepeat(-1);
		}
	}

	public GameObject avatarRecursive;

	public GameObject avatar2dRecursive;

	public RectTransform wingPersonPanel;

	public RectTransform textField;

	public GameObject avatarMove;

	public Transform[] movePts;

	public GameObject[] avatarSpeed;

	public GameObject[] avatarSpeed2;

	private Vector3[] circleSm = new Vector3[]
	{
		new Vector3(16f, 0f, 0f),
		new Vector3(14.56907f, 8.009418f, 0f),
		new Vector3(15.96541f, 4.638379f, 0f),
		new Vector3(11.31371f, 11.31371f, 0f),
		new Vector3(11.31371f, 11.31371f, 0f),
		new Vector3(4.638379f, 15.96541f, 0f),
		new Vector3(8.009416f, 14.56908f, 0f),
		new Vector3(-6.993822E-07f, 16f, 0f),
		new Vector3(-6.993822E-07f, 16f, 0f),
		new Vector3(-8.009419f, 14.56907f, 0f),
		new Vector3(-4.63838f, 15.9654f, 0f),
		new Vector3(-11.31371f, 11.31371f, 0f),
		new Vector3(-11.31371f, 11.31371f, 0f),
		new Vector3(-15.9654f, 4.63838f, 0f),
		new Vector3(-14.56908f, 8.009415f, 0f),
		new Vector3(-16f, -1.398764E-06f, 0f),
		new Vector3(-16f, -1.398764E-06f, 0f),
		new Vector3(-14.56907f, -8.009418f, 0f),
		new Vector3(-15.9654f, -4.638382f, 0f),
		new Vector3(-11.31371f, -11.31371f, 0f),
		new Vector3(-11.31371f, -11.31371f, 0f),
		new Vector3(-4.638381f, -15.9654f, 0f),
		new Vector3(-8.009413f, -14.56908f, 0f),
		new Vector3(1.907981E-07f, -16f, 0f),
		new Vector3(1.907981E-07f, -16f, 0f),
		new Vector3(8.00942f, -14.56907f, 0f),
		new Vector3(4.638381f, -15.9654f, 0f),
		new Vector3(11.31371f, -11.3137f, 0f),
		new Vector3(11.31371f, -11.3137f, 0f),
		new Vector3(15.96541f, -4.638378f, 0f),
		new Vector3(14.56907f, -8.009418f, 0f),
		new Vector3(16f, 2.797529E-06f, 0f)
	};

	private Vector3[] circleLrg = new Vector3[]
	{
		new Vector3(25f, 0f, 0f),
		new Vector3(22.76418f, 12.51472f, 0f),
		new Vector3(24.94595f, 7.247467f, 0f),
		new Vector3(17.67767f, 17.67767f, 0f),
		new Vector3(17.67767f, 17.67767f, 0f),
		new Vector3(7.247467f, 24.94595f, 0f),
		new Vector3(12.51471f, 22.76418f, 0f),
		new Vector3(-1.092785E-06f, 25f, 0f),
		new Vector3(-1.092785E-06f, 25f, 0f),
		new Vector3(-12.51472f, 22.76418f, 0f),
		new Vector3(-7.247468f, 24.94594f, 0f),
		new Vector3(-17.67767f, 17.67767f, 0f),
		new Vector3(-17.67767f, 17.67767f, 0f),
		new Vector3(-24.94594f, 7.247468f, 0f),
		new Vector3(-22.76418f, 12.51471f, 0f),
		new Vector3(-25f, -2.185569E-06f, 0f),
		new Vector3(-25f, -2.185569E-06f, 0f),
		new Vector3(-22.76418f, -12.51472f, 0f),
		new Vector3(-24.94594f, -7.247472f, 0f),
		new Vector3(-17.67767f, -17.67767f, 0f),
		new Vector3(-17.67767f, -17.67767f, 0f),
		new Vector3(-7.247469f, -24.94594f, 0f),
		new Vector3(-12.51471f, -22.76418f, 0f),
		new Vector3(2.98122E-07f, -25f, 0f),
		new Vector3(2.98122E-07f, -25f, 0f),
		new Vector3(12.51472f, -22.76418f, 0f),
		new Vector3(7.24747f, -24.94594f, 0f),
		new Vector3(17.67768f, -17.67766f, 0f),
		new Vector3(17.67768f, -17.67766f, 0f),
		new Vector3(24.94595f, -7.247465f, 0f),
		new Vector3(22.76418f, -12.51472f, 0f),
		new Vector3(25f, 4.371139E-06f, 0f)
	};
}
