using System;
using System.Collections;

namespace LitJson
{
	public interface IJsonWrapper : IList, IOrderedDictionary, ICollection, IEnumerable, IDictionary
	{
		bool IsArray { get; }

		bool IsBoolean { get; }

		bool IsDouble { get; }

		bool IsInt { get; }

		bool IsLong { get; }

		bool IsObject { get; }

		bool IsString { get; }

		bool GetBoolean();

		double GetDouble();

		int GetInt();

		JsonType GetJsonType();

		long GetLong();

		string GetString();

		void SetBoolean(bool val);

		void SetDouble(double val);

		void SetInt(int val);

		void SetJsonType(JsonType type);

		void SetLong(long val);

		void SetString(string val);

		string ToJson();

		void ToJson(JsonWriter writer);
	}
}
