using System;

namespace I2.Loc.SimpleJSON
{
	public static class JSON
	{
		public static JSONNode Parse(string aJSON)
		{
			return JSONNode.Parse(aJSON);
		}
	}
}
