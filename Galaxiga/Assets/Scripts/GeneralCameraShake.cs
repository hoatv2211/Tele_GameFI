using System;
using UnityEngine;

public class GeneralCameraShake : MonoBehaviour
{
	private void Start()
	{
		this.avatarBig = GameObject.Find("AvatarBig");
		AnimationCurve volume = new AnimationCurve(new Keyframe[]
		{
			new Keyframe(8.130963E-06f, 0.06526042f, 0f, -1f),
			new Keyframe(0.0007692695f, 2.449077f, 9.078861f, 9.078861f),
			new Keyframe(0.01541314f, 0.9343268f, -40f, -40f),
			new Keyframe(0.05169491f, 0.03835937f, -0.08621139f, -0.08621139f)
		});
		AnimationCurve frequency = new AnimationCurve(new Keyframe[]
		{
			new Keyframe(0f, 0.003005181f, 0f, 0f),
			new Keyframe(0.01507768f, 0.002227979f, 0f, 0f)
		});
		this.boomAudioClip = LeanAudio.createAudio(volume, frequency, LeanAudio.options().setVibrato(new Vector3[]
		{
			new Vector3(0.1f, 0f, 0f)
		}));
		this.bigGuyJump();
	}

	private void bigGuyJump()
	{
		float height = Mathf.PerlinNoise(this.jumpIter, 0f) * 10f;
		height = height * height * 0.3f;
		LeanTween.moveY(this.avatarBig, height, 1f).setEase(LeanTweenType.easeInOutQuad).setOnComplete(delegate()
		{
			LeanTween.moveY(this.avatarBig, 0f, 0.27f).setEase(LeanTweenType.easeInQuad).setOnComplete(delegate()
			{
				LeanTween.cancel(this.gameObject);
				float num = height * 0.2f;
				float time = 0.42f;
				float time2 = 1.6f;
				LTDescr shakeTween = LeanTween.rotateAroundLocal(this.gameObject, Vector3.right, num, time).setEase(LeanTweenType.easeShake).setLoopClamp().setRepeat(-1);
				LeanTween.value(this.gameObject, num, 0f, time2).setOnUpdate(delegate(float val)
				{
					shakeTween.setTo(Vector3.right * val);
				}).setEase(LeanTweenType.easeOutQuad);
				GameObject[] array = GameObject.FindGameObjectsWithTag("Respawn");
				foreach (GameObject gameObject in array)
				{
					gameObject.GetComponent<Rigidbody>().AddForce(Vector3.up * 100f * height);
				}
				GameObject[] array3 = GameObject.FindGameObjectsWithTag("GameController");
				foreach (GameObject gameObject2 in array3)
				{
					float num2 = gameObject2.transform.eulerAngles.z;
					num2 = (float)((num2 <= 0f || num2 >= 180f) ? -1 : 1);
					gameObject2.GetComponent<Rigidbody>().AddForce(new Vector3(num2, 0f, 0f) * 15f * height);
				}
				LeanAudio.play(this.boomAudioClip, this.transform.position, height * 0.2f);
				LeanTween.delayedCall(2f, new Action(this.bigGuyJump));
			});
		});
		this.jumpIter += 5.2f;
	}

	private GameObject avatarBig;

	private float jumpIter = 9.5f;

	private AudioClip boomAudioClip;
}
