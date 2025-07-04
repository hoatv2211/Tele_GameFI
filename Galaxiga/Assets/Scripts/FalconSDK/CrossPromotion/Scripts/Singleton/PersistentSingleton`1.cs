using System;
using UnityEngine;

namespace FalconSDK.CrossPromotion.Scripts.Singleton
{
	public class PersistentSingleton<T> : MonoBehaviour where T : Component
	{
		public static T Instance
		{
			get
			{
				if (PersistentSingleton<T>._instance == null)
				{
					PersistentSingleton<T>._instance = UnityEngine.Object.FindObjectOfType<T>();
					if (PersistentSingleton<T>._instance == null)
					{
						GameObject gameObject = new GameObject();
						PersistentSingleton<T>._instance = gameObject.AddComponent<T>();
					}
				}
				return PersistentSingleton<T>._instance;
			}
		}

		protected virtual void Awake()
		{
			if (PersistentSingleton<T>._instance == null)
			{
				PersistentSingleton<T>._instance = (this as T);
				UnityEngine.Object.DontDestroyOnLoad(base.transform.gameObject);
			}
			else if (this != PersistentSingleton<T>._instance)
			{
				UnityEngine.Object.Destroy(base.gameObject);
			}
		}

		protected static T _instance;
	}
}
