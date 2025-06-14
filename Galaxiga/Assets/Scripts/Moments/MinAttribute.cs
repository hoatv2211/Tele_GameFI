using System;
using UnityEngine;

namespace Moments
{
	public sealed class MinAttribute : PropertyAttribute
	{
		public MinAttribute(float min)
		{
			this.min = min;
		}

		public readonly float min;
	}
}
