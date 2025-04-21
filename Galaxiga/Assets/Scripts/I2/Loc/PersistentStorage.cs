using System;

namespace I2.Loc
{
	public static class PersistentStorage
	{
		public static void SetSetting_String(string key, string value)
		{
			if (PersistentStorage.mStorage == null)
			{
				PersistentStorage.mStorage = new I2CustomPersistentStorage();
			}
			PersistentStorage.mStorage.SetSetting_String(key, value);
		}

		public static string GetSetting_String(string key, string defaultValue)
		{
			if (PersistentStorage.mStorage == null)
			{
				PersistentStorage.mStorage = new I2CustomPersistentStorage();
			}
			return PersistentStorage.mStorage.GetSetting_String(key, defaultValue);
		}

		public static void DeleteSetting(string key)
		{
			if (PersistentStorage.mStorage == null)
			{
				PersistentStorage.mStorage = new I2CustomPersistentStorage();
			}
			PersistentStorage.mStorage.DeleteSetting(key);
		}

		public static bool HasSetting(string key)
		{
			if (PersistentStorage.mStorage == null)
			{
				PersistentStorage.mStorage = new I2CustomPersistentStorage();
			}
			return PersistentStorage.mStorage.HasSetting(key);
		}

		public static void ForceSaveSettings()
		{
			if (PersistentStorage.mStorage == null)
			{
				PersistentStorage.mStorage = new I2CustomPersistentStorage();
			}
			PersistentStorage.mStorage.ForceSaveSettings();
		}

		public static bool CanAccessFiles()
		{
			if (PersistentStorage.mStorage == null)
			{
				PersistentStorage.mStorage = new I2CustomPersistentStorage();
			}
			return PersistentStorage.mStorage.CanAccessFiles();
		}

		public static bool SaveFile(PersistentStorage.eFileType fileType, string fileName, string data, bool logExceptions = true)
		{
			if (PersistentStorage.mStorage == null)
			{
				PersistentStorage.mStorage = new I2CustomPersistentStorage();
			}
			return PersistentStorage.mStorage.SaveFile(fileType, fileName, data, logExceptions);
		}

		public static string LoadFile(PersistentStorage.eFileType fileType, string fileName, bool logExceptions = true)
		{
			if (PersistentStorage.mStorage == null)
			{
				PersistentStorage.mStorage = new I2CustomPersistentStorage();
			}
			return PersistentStorage.mStorage.LoadFile(fileType, fileName, logExceptions);
		}

		public static bool DeleteFile(PersistentStorage.eFileType fileType, string fileName, bool logExceptions = true)
		{
			if (PersistentStorage.mStorage == null)
			{
				PersistentStorage.mStorage = new I2CustomPersistentStorage();
			}
			return PersistentStorage.mStorage.DeleteFile(fileType, fileName, logExceptions);
		}

		public static bool HasFile(PersistentStorage.eFileType fileType, string fileName, bool logExceptions = true)
		{
			if (PersistentStorage.mStorage == null)
			{
				PersistentStorage.mStorage = new I2CustomPersistentStorage();
			}
			return PersistentStorage.mStorage.HasFile(fileType, fileName, logExceptions);
		}

		private static I2CustomPersistentStorage mStorage;

		public enum eFileType
		{
			Raw,
			Persistent,
			Temporal,
			Streaming
		}
	}
}
