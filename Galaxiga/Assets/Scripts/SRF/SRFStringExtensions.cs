using System;

namespace SRF
{
	public static class SRFStringExtensions
	{
		public static string Fmt(this string formatString, params object[] args)
		{
			return string.Format(formatString, args);
		}
	}
}
