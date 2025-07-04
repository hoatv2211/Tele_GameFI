using System;
using System.Collections.Generic;
using OneSignalPush.MiniJSON;
using UnityEngine;

public class OneSignalAndroid : OneSignalPlatform
{
	public OneSignalAndroid(string gameObjectName, string googleProjectNumber, string appId, OneSignal.OSInFocusDisplayOption displayOption, OneSignal.LOG_LEVEL logLevel, OneSignal.LOG_LEVEL visualLevel, bool requiresUserConsent)
	{
		OneSignalAndroid.mOneSignal = new AndroidJavaObject("com.onesignal.OneSignalUnityProxy", new object[]
		{
			gameObjectName,
			googleProjectNumber,
			appId,
			(int)logLevel,
			(int)visualLevel,
			requiresUserConsent
		});
		this.SetInFocusDisplaying(displayOption);
	}

	public void SetLogLevel(OneSignal.LOG_LEVEL logLevel, OneSignal.LOG_LEVEL visualLevel)
	{
		OneSignalAndroid.mOneSignal.Call("setLogLevel", new object[]
		{
			(int)logLevel,
			(int)visualLevel
		});
	}

	public void SetLocationShared(bool shared)
	{
		OneSignalAndroid.mOneSignal.Call("setLocationShared", new object[]
		{
			shared
		});
	}

	public void SendTag(string tagName, string tagValue)
	{
		OneSignalAndroid.mOneSignal.Call("sendTag", new object[]
		{
			tagName,
			tagValue
		});
	}

	public void SendTags(IDictionary<string, string> tags)
	{
		OneSignalAndroid.mOneSignal.Call("sendTags", new object[]
		{
			Json.Serialize(tags)
		});
	}

	public void GetTags()
	{
		OneSignalAndroid.mOneSignal.Call("getTags", Array.Empty<object>());
	}

	public void DeleteTag(string key)
	{
		OneSignalAndroid.mOneSignal.Call("deleteTag", new object[]
		{
			key
		});
	}

	public void DeleteTags(IList<string> keys)
	{
		OneSignalAndroid.mOneSignal.Call("deleteTags", new object[]
		{
			Json.Serialize(keys)
		});
	}

	public void IdsAvailable()
	{
		OneSignalAndroid.mOneSignal.Call("idsAvailable", Array.Empty<object>());
	}

	public void RegisterForPushNotifications()
	{
	}

	public void promptForPushNotificationsWithUserResponse()
	{
	}

	public void EnableVibrate(bool enable)
	{
		OneSignalAndroid.mOneSignal.Call("enableVibrate", new object[]
		{
			enable
		});
	}

	public void EnableSound(bool enable)
	{
		OneSignalAndroid.mOneSignal.Call("enableSound", new object[]
		{
			enable
		});
	}

	public void SetInFocusDisplaying(OneSignal.OSInFocusDisplayOption display)
	{
		OneSignalAndroid.mOneSignal.Call("setInFocusDisplaying", new object[]
		{
			(int)display
		});
	}

	public void SetSubscription(bool enable)
	{
		OneSignalAndroid.mOneSignal.Call("setSubscription", new object[]
		{
			enable
		});
	}

	public void PostNotification(Dictionary<string, object> data)
	{
		OneSignalAndroid.mOneSignal.Call("postNotification", new object[]
		{
			Json.Serialize(data)
		});
	}

	public void SyncHashedEmail(string email)
	{
		OneSignalAndroid.mOneSignal.Call("syncHashedEmail", new object[]
		{
			email
		});
	}

	public void PromptLocation()
	{
		OneSignalAndroid.mOneSignal.Call("promptLocation", Array.Empty<object>());
	}

	public void ClearOneSignalNotifications()
	{
		OneSignalAndroid.mOneSignal.Call("clearOneSignalNotifications", Array.Empty<object>());
	}

	public void addPermissionObserver()
	{
		OneSignalAndroid.mOneSignal.Call("addPermissionObserver", Array.Empty<object>());
	}

	public void removePermissionObserver()
	{
		OneSignalAndroid.mOneSignal.Call("removePermissionObserver", Array.Empty<object>());
	}

	public void addSubscriptionObserver()
	{
		OneSignalAndroid.mOneSignal.Call("addSubscriptionObserver", Array.Empty<object>());
	}

	public void removeSubscriptionObserver()
	{
		OneSignalAndroid.mOneSignal.Call("removeSubscriptionObserver", Array.Empty<object>());
	}

	public void addEmailSubscriptionObserver()
	{
		OneSignalAndroid.mOneSignal.Call("addEmailSubscriptionObserver", Array.Empty<object>());
	}

	public void removeEmailSubscriptionObserver()
	{
		OneSignalAndroid.mOneSignal.Call("removeEmailSubscriptionObserver", Array.Empty<object>());
	}

	public void UserDidProvideConsent(bool consent)
	{
		OneSignalAndroid.mOneSignal.Call("provideUserConsent", new object[]
		{
			consent
		});
	}

	public bool UserProvidedConsent()
	{
		return OneSignalAndroid.mOneSignal.Call<bool>("userProvidedPrivacyConsent", Array.Empty<object>());
	}

	public void SetRequiresUserPrivacyConsent(bool required)
	{
		OneSignalAndroid.mOneSignal.Call("setRequiresUserPrivacyConsent", new object[]
		{
			required
		});
	}

	public OSPermissionSubscriptionState getPermissionSubscriptionState()
	{
		return OneSignalPlatformHelper.parsePermissionSubscriptionState(this, OneSignalAndroid.mOneSignal.Call<string>("getPermissionSubscriptionState", Array.Empty<object>()));
	}

	public OSPermissionStateChanges parseOSPermissionStateChanges(string jsonStat)
	{
		return OneSignalPlatformHelper.parseOSPermissionStateChanges(this, jsonStat);
	}

	public OSSubscriptionStateChanges parseOSSubscriptionStateChanges(string jsonStat)
	{
		return OneSignalPlatformHelper.parseOSSubscriptionStateChanges(this, jsonStat);
	}

	public OSEmailSubscriptionStateChanges parseOSEmailSubscriptionStateChanges(string jsonState)
	{
		return OneSignalPlatformHelper.parseOSEmailSubscriptionStateChanges(this, jsonState);
	}

	public OSPermissionState parseOSPermissionState(object stateDict)
	{
		Dictionary<string, object> dictionary = stateDict as Dictionary<string, object>;
		OSPermissionState ospermissionState = new OSPermissionState();
		ospermissionState.hasPrompted = true;
		bool flag = Convert.ToBoolean(dictionary["enabled"]);
		ospermissionState.status = ((!flag) ? OSNotificationPermission.Denied : OSNotificationPermission.Authorized);
		return ospermissionState;
	}

	public OSSubscriptionState parseOSSubscriptionState(object stateDict)
	{
		Dictionary<string, object> dictionary = stateDict as Dictionary<string, object>;
		return new OSSubscriptionState
		{
			subscribed = Convert.ToBoolean(dictionary["subscribed"]),
			userSubscriptionSetting = Convert.ToBoolean(dictionary["userSubscriptionSetting"]),
			userId = (dictionary["userId"] as string),
			pushToken = (dictionary["pushToken"] as string)
		};
	}

	public OSEmailSubscriptionState parseOSEmailSubscriptionState(object stateDict)
	{
		Dictionary<string, object> dictionary = stateDict as Dictionary<string, object>;
		return new OSEmailSubscriptionState
		{
			subscribed = Convert.ToBoolean(dictionary["subscribed"]),
			emailUserId = (dictionary["emailUserId"] as string),
			emailAddress = (dictionary["emailAddress"] as string)
		};
	}

	public void SetEmail(string email, string emailAuthCode)
	{
		OneSignalAndroid.mOneSignal.Call("setEmail", new object[]
		{
			email,
			emailAuthCode
		});
	}

	public void SetEmail(string email)
	{
		AndroidJavaObject androidJavaObject = OneSignalAndroid.mOneSignal;
		string methodName = "setEmail";
		object[] array = new object[2];
		array[0] = email;
		androidJavaObject.Call(methodName, array);
	}

	public void LogoutEmail()
	{
		OneSignalAndroid.mOneSignal.Call("logoutEmail", Array.Empty<object>());
	}

	private static AndroidJavaObject mOneSignal;
}
