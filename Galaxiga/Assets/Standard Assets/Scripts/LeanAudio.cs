using System;
using System.Runtime.CompilerServices;
using UnityEngine;

public class LeanAudio
{
	public static LeanAudioOptions options()
	{
		if (LeanAudio.generatedWaveDistances == null)
		{
			LeanAudio.generatedWaveDistances = new float[LeanAudio.PROCESSING_ITERATIONS_MAX];
			LeanAudio.longList = new float[LeanAudio.PROCESSING_ITERATIONS_MAX];
		}
		return new LeanAudioOptions();
	}

	public static LeanAudioStream createAudioStream(AnimationCurve volume, AnimationCurve frequency, LeanAudioOptions options = null)
	{
		if (options == null)
		{
			options = new LeanAudioOptions();
		}
		options.useSetData = false;
		int waveLength = LeanAudio.createAudioWave(volume, frequency, options);
		LeanAudio.createAudioFromWave(waveLength, options);
		return options.stream;
	}

	public static AudioClip createAudio(AnimationCurve volume, AnimationCurve frequency, LeanAudioOptions options = null)
	{
		if (options == null)
		{
			options = new LeanAudioOptions();
		}
		int waveLength = LeanAudio.createAudioWave(volume, frequency, options);
		return LeanAudio.createAudioFromWave(waveLength, options);
	}

	private static int createAudioWave(AnimationCurve volume, AnimationCurve frequency, LeanAudioOptions options)
	{
		float time = volume[volume.length - 1].time;
		int num = 0;
		float num2 = 0f;
		for (int i = 0; i < LeanAudio.PROCESSING_ITERATIONS_MAX; i++)
		{
			float num3 = frequency.Evaluate(num2);
			if (num3 < LeanAudio.MIN_FREQEUNCY_PERIOD)
			{
				num3 = LeanAudio.MIN_FREQEUNCY_PERIOD;
			}
			float num4 = volume.Evaluate(num2 + 0.5f * num3);
			if (options.vibrato != null)
			{
				for (int j = 0; j < options.vibrato.Length; j++)
				{
					float num5 = Mathf.Abs(Mathf.Sin(1.5708f + num2 * (1f / options.vibrato[j][0]) * 3.14159274f));
					float num6 = 1f - options.vibrato[j][1];
					num5 = options.vibrato[j][1] + num6 * num5;
					num4 *= num5;
				}
			}
			if (num2 + 0.5f * num3 >= time)
			{
				break;
			}
			if (num >= LeanAudio.PROCESSING_ITERATIONS_MAX - 1)
			{
				UnityEngine.Debug.LogError("LeanAudio has reached it's processing cap. To avoid this error increase the number of iterations ex: LeanAudio.PROCESSING_ITERATIONS_MAX = " + LeanAudio.PROCESSING_ITERATIONS_MAX * 2);
				break;
			}
			int num7 = num / 2;
			num2 += num3;
			LeanAudio.generatedWaveDistances[num7] = num2;
			LeanAudio.longList[num] = num2;
			LeanAudio.longList[num + 1] = ((i % 2 != 0) ? num4 : (-num4));
			num += 2;
		}
		num += -2;
		LeanAudio.generatedWaveDistancesCount = num / 2;
		return num;
	}

	private static AudioClip createAudioFromWave(int waveLength, LeanAudioOptions options)
	{
		float num = LeanAudio.longList[waveLength - 2];
		float[] array = new float[(int)((float)options.frequencyRate * num)];
		int num2 = 0;
		float num3 = LeanAudio.longList[num2];
		float num4 = 0f;
		float num5 = LeanAudio.longList[num2];
		float num6 = LeanAudio.longList[num2 + 1];
		for (int i = 0; i < array.Length; i++)
		{
			float num7 = (float)i / (float)options.frequencyRate;
			if (num7 > LeanAudio.longList[num2])
			{
				num4 = LeanAudio.longList[num2];
				num2 += 2;
				num3 = LeanAudio.longList[num2] - LeanAudio.longList[num2 - 2];
				num6 = LeanAudio.longList[num2 + 1];
			}
			num5 = num7 - num4;
			float num8 = num5 / num3;
			float num9 = Mathf.Sin(num8 * 3.14159274f);
			if (options.waveStyle == LeanAudioOptions.LeanAudioWaveStyle.Square)
			{
				if (num9 > 0f)
				{
					num9 = 1f;
				}
				if (num9 < 0f)
				{
					num9 = -1f;
				}
			}
			else if (options.waveStyle == LeanAudioOptions.LeanAudioWaveStyle.Sawtooth)
			{
				float num10 = (num9 <= 0f) ? -1f : 1f;
				if (num8 < 0.5f)
				{
					num9 = num8 * 2f * num10;
				}
				else
				{
					num9 = (1f - num8) * 2f * num10;
				}
			}
			else if (options.waveStyle == LeanAudioOptions.LeanAudioWaveStyle.Noise)
			{
				float num11 = 1f - options.waveNoiseInfluence + Mathf.PerlinNoise(0f, num7 * options.waveNoiseScale) * options.waveNoiseInfluence;
				num9 *= num11;
			}
			num9 *= num6;
			if (options.modulation != null)
			{
				for (int j = 0; j < options.modulation.Length; j++)
				{
					float num12 = Mathf.Abs(Mathf.Sin(1.5708f + num7 * (1f / options.modulation[j][0]) * 3.14159274f));
					float num13 = 1f - options.modulation[j][1];
					num12 = options.modulation[j][1] + num13 * num12;
					num9 *= num12;
				}
			}
			array[i] = num9;
		}
		int num14 = array.Length;
		AudioClip audioClip;
		if (options.useSetData)
		{
			string name = "Generated Audio";
			int lengthSamples = num14;
			int channels = 1;
			int frequencyRate = options.frequencyRate;
			bool stream = false;
			AudioClip.PCMReaderCallback pcmreadercallback = null;
			if (LeanAudio._003C_003Ef__mg_0024cache0 == null)
			{
				LeanAudio._003C_003Ef__mg_0024cache0 = new AudioClip.PCMSetPositionCallback(LeanAudio.OnAudioSetPosition);
			}
			audioClip = AudioClip.Create(name, lengthSamples, channels, frequencyRate, stream, pcmreadercallback, LeanAudio._003C_003Ef__mg_0024cache0);
			audioClip.SetData(array, 0);
		}
		else
		{
			options.stream = new LeanAudioStream(array);
			audioClip = AudioClip.Create("Generated Audio", num14, 1, options.frequencyRate, false, new AudioClip.PCMReaderCallback(options.stream.OnAudioRead), new AudioClip.PCMSetPositionCallback(options.stream.OnAudioSetPosition));
			options.stream.audioClip = audioClip;
		}
		return audioClip;
	}

	private static void OnAudioSetPosition(int newPosition)
	{
	}

	public static AudioClip generateAudioFromCurve(AnimationCurve curve, int frequencyRate = 44100)
	{
		float time = curve[curve.length - 1].time;
		float num = time;
		float[] array = new float[(int)((float)frequencyRate * num)];
		for (int i = 0; i < array.Length; i++)
		{
			float time2 = (float)i / (float)frequencyRate;
			array[i] = curve.Evaluate(time2);
		}
		int lengthSamples = array.Length;
		AudioClip audioClip = AudioClip.Create("Generated Audio", lengthSamples, 1, frequencyRate, false);
		audioClip.SetData(array, 0);
		return audioClip;
	}

	public static AudioSource play(AudioClip audio, float volume)
	{
		AudioSource audioSource = LeanAudio.playClipAt(audio, Vector3.zero);
		audioSource.volume = volume;
		return audioSource;
	}

	public static AudioSource play(AudioClip audio)
	{
		return LeanAudio.playClipAt(audio, Vector3.zero);
	}

	public static AudioSource play(AudioClip audio, Vector3 pos)
	{
		return LeanAudio.playClipAt(audio, pos);
	}

	public static AudioSource play(AudioClip audio, Vector3 pos, float volume)
	{
		AudioSource audioSource = LeanAudio.playClipAt(audio, pos);
		audioSource.minDistance = 1f;
		audioSource.volume = volume;
		return audioSource;
	}

	public static AudioSource playClipAt(AudioClip clip, Vector3 pos)
	{
		GameObject gameObject = new GameObject();
		gameObject.transform.position = pos;
		AudioSource audioSource = gameObject.AddComponent<AudioSource>();
		audioSource.clip = clip;
		audioSource.Play();
		UnityEngine.Object.Destroy(gameObject, clip.length);
		return audioSource;
	}

	public static void printOutAudioClip(AudioClip audioClip, ref AnimationCurve curve, float scaleX = 1f)
	{
		float[] array = new float[audioClip.samples * audioClip.channels];
		audioClip.GetData(array, 0);
		int i = 0;
		Keyframe[] array2 = new Keyframe[array.Length];
		while (i < array.Length)
		{
			array2[i] = new Keyframe((float)i * scaleX, array[i]);
			i++;
		}
		curve = new AnimationCurve(array2);
	}

	public static float MIN_FREQEUNCY_PERIOD = 0.000115f;

	public static int PROCESSING_ITERATIONS_MAX = 50000;

	public static float[] generatedWaveDistances;

	public static int generatedWaveDistancesCount;

	private static float[] longList;

	[CompilerGenerated]
	private static AudioClip.PCMSetPositionCallback _003C_003Ef__mg_0024cache0;
}
