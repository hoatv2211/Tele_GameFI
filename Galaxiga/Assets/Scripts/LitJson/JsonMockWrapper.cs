using System;
using System.Collections;

namespace LitJson
{
	public class JsonMockWrapper : IJsonWrapper, IList, IOrderedDictionary, ICollection, IEnumerable, IDictionary
	{
		public bool IsArray
		{
			get
			{
				return false;
			}
		}

		public bool IsBoolean
		{
			get
			{
				return false;
			}
		}

		public bool IsDouble
		{
			get
			{
				return false;
			}
		}

		public bool IsInt
		{
			get
			{
				return false;
			}
		}

		public bool IsLong
		{
			get
			{
				return false;
			}
		}

		public bool IsObject
		{
			get
			{
				return false;
			}
		}

		public bool IsString
		{
			get
			{
				return false;
			}
		}

		public bool GetBoolean()
		{
			return false;
		}

		public double GetDouble()
		{
			return 0.0;
		}

		public int GetInt()
		{
			return 0;
		}

		public JsonType GetJsonType()
		{
			return JsonType.None;
		}

		public long GetLong()
		{
			return 0L;
		}

		public string GetString()
		{
			return string.Empty;
		}

		public void SetBoolean(bool val)
		{
		}

		public void SetDouble(double val)
		{
		}

		public void SetInt(int val)
		{
		}

		public void SetJsonType(JsonType type)
		{
		}

		public void SetLong(long val)
		{
		}

		public void SetString(string val)
		{
		}

		public string ToJson()
		{
			return string.Empty;
		}

		public void ToJson(JsonWriter writer)
		{
		}

		bool IList.IsFixedSize
		{
			get
			{
				return true;
			}
		}

		bool IList.IsReadOnly
		{
			get
			{
				return true;
			}
		}

		object IList.this[int index]
		{
			get
			{
				return null;
			}
			set
			{
			}
		}

		int IList.Add(object value)
		{
			return 0;
		}

		void IList.Clear()
		{
		}

		bool IList.Contains(object value)
		{
			return false;
		}

		int IList.IndexOf(object value)
		{
			return -1;
		}

		void IList.Insert(int i, object v)
		{
		}

		void IList.Remove(object value)
		{
		}

		void IList.RemoveAt(int index)
		{
		}

		int ICollection.Count
		{
			get
			{
				return 0;
			}
		}

		bool ICollection.IsSynchronized
		{
			get
			{
				return false;
			}
		}

		object ICollection.SyncRoot
		{
			get
			{
				return null;
			}
		}

		void ICollection.CopyTo(Array array, int index)
		{
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return null;
		}

		bool IDictionary.IsFixedSize
		{
			get
			{
				return true;
			}
		}

		bool IDictionary.IsReadOnly
		{
			get
			{
				return true;
			}
		}

		ICollection IDictionary.Keys
		{
			get
			{
				return null;
			}
		}

		ICollection IDictionary.Values
		{
			get
			{
				return null;
			}
		}

		object IDictionary.this[object key]
		{
			get
			{
				return null;
			}
			set
			{
			}
		}

		void IDictionary.Add(object k, object v)
		{
		}

		void IDictionary.Clear()
		{
		}

		bool IDictionary.Contains(object key)
		{
			return false;
		}

		void IDictionary.Remove(object key)
		{
		}

		IDictionaryEnumerator IDictionary.GetEnumerator()
		{
			return null;
		}

		object IOrderedDictionary.this[int idx]
		{
			get
			{
				return null;
			}
			set
			{
			}
		}

		IDictionaryEnumerator IOrderedDictionary.GetEnumerator()
		{
			return null;
		}

		void IOrderedDictionary.Insert(int i, object k, object v)
		{
		}

		void IOrderedDictionary.RemoveAt(int i)
		{
		}
	}
}
