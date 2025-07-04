using System;
using System.Text;

namespace I2.Loc
{
	public class StringObfucator
	{
		public static string Encode(string NormalString)
		{
			string result;
			try
			{
				string regularString = StringObfucator.XoREncode(NormalString);
				result = StringObfucator.ToBase64(regularString);
			}
			catch (Exception)
			{
				result = null;
			}
			return result;
		}

		public static string Decode(string ObfucatedString)
		{
			string result;
			try
			{
				string normalString = StringObfucator.FromBase64(ObfucatedString);
				result = StringObfucator.XoREncode(normalString);
			}
			catch (Exception)
			{
				result = null;
			}
			return result;
		}

		private static string ToBase64(string regularString)
		{
			byte[] bytes = Encoding.UTF8.GetBytes(regularString);
			return Convert.ToBase64String(bytes);
		}

		private static string FromBase64(string base64string)
		{
			byte[] array = Convert.FromBase64String(base64string);
			return Encoding.UTF8.GetString(array, 0, array.Length);
		}

		private static string XoREncode(string NormalString)
		{
            try
            {
                char[] stringObfuscatorPassword = StringObfuscatorPassword;
                char[] array = NormalString.ToCharArray();
                int num = stringObfuscatorPassword.Length;
                int i = 0;
                for (int num2 = array.Length; i < num2; i++)
                {
                    array[i] = (char)(array[i] ^ stringObfuscatorPassword[i % num] ^ (byte)((i % 2 != 0) ? (-i * 51) : (i * 23)));
                }
                return new string(array);
            }
            catch (Exception)
            {
                return null;
            }
        }

		public static char[] StringObfuscatorPassword = "ÝúbUu\u0010\u008b¸CÁÂ§*4\u0013PÚ©-á©¾@T6D\u0089l\u000f±\u0091ÒWâuzÅm4GÐó\u0019Ø$=Í g,¥Q\u0083\të®iKEß r¡\u0019\u009f×6\u00160Ít \u00904öÃ\u0093~^«\u001cy\u0003:\u0090\u0096Èd\u0010\u008f1\u001a<Q\u008f\u0099ÛÝúbUu\u0010\u008b¸CÁÂ§*4\u0013PÚ©-á©¾@T6D\u0089l\u000f±\u0091ÒWâuzÅm4GÐó\u0019Ø$=Í g,¥Q\u0083\të®iKEß r¡\u0019\u009f×6\u00160Ít \u00904öÃ\u0093~^«\u001cy\u0003:\u0090\u0096Èd".ToCharArray();
	}
}
