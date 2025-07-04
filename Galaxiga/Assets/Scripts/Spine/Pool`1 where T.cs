using System;
using System.Collections.Generic;

namespace Spine
{
	public class Pool<T> where T : class, new()
	{
		public Pool(int initialCapacity = 16, int max = 2147483647)
		{
			this.freeObjects = new Stack<T>(initialCapacity);
			this.max = max;
		}

		public int Count
		{
			get
			{
				return this.freeObjects.Count;
			}
		}

		public int Peak { get; private set; }

		public T Obtain()
		{
			return (this.freeObjects.Count != 0) ? this.freeObjects.Pop() : Activator.CreateInstance<T>();
		}

		public void Free(T obj)
		{
			if (obj == null)
			{
				throw new ArgumentNullException("obj", "obj cannot be null");
			}
			if (this.freeObjects.Count < this.max)
			{
				this.freeObjects.Push(obj);
				this.Peak = Math.Max(this.Peak, this.freeObjects.Count);
			}
			this.Reset(obj);
		}

		public void Clear()
		{
			this.freeObjects.Clear();
		}

		protected void Reset(T obj)
		{
			Pool<T>.IPoolable poolable = obj as Pool<T>.IPoolable;
			if (poolable != null)
			{
				poolable.Reset();
			}
		}

		public readonly int max;

		private readonly Stack<T> freeObjects;

		public interface IPoolable
		{
			void Reset();
		}
	}
}
