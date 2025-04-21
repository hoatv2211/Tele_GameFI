using System;
using System.Collections;
using UnityEngine;

namespace I2.Loc
{
	public class CoroutineManager : MonoBehaviour
	{
		private static CoroutineManager pInstance
		{
			get
			{
				if (CoroutineManager.mInstance == null)
				{
					GameObject gameObject = new GameObject("_Coroutiner");
					gameObject.hideFlags = HideFlags.HideAndDontSave;
					CoroutineManager.mInstance = gameObject.AddComponent<CoroutineManager>();
					if (Application.isPlaying)
					{
						UnityEngine.Object.DontDestroyOnLoad(gameObject);
					}
				}
				return CoroutineManager.mInstance;
			}
		}

		private void Awake()
		{
			if (Application.isPlaying)
			{
				UnityEngine.Object.DontDestroyOnLoad(base.gameObject);
			}
		}

		public static Coroutine Start(IEnumerator coroutine)
		{
			return CoroutineManager.pInstance.StartCoroutine(coroutine);
		}

		private static CoroutineManager mInstance;
	}
}
