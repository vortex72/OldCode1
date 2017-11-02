using System;

namespace EPWI.Components.Utility
{
  public static class StringUtility
  {
    public static string TrimAndConvertNulls(this string value)
    {
      value = value ?? string.Empty;
      return value.Trim();
    }

    public static string Left(this string value, int length)
    {
      string result = null;

      if (value != null)
      {
        result = value.Substring(0, Math.Min(value.Length, length));
      }
      return result;
    }

    public static DateTime AS400DateToDate(this string value)
    {
      if (!string.IsNullOrEmpty(value))
      {
        return new DateTime(int.Parse(value.Substring(0, 4)), int.Parse(value.Substring(4, 2)), int.Parse(value.Substring(6, 2)));
      }
      else
      {
        return DateTime.MinValue;
      }
    }

    public static string ToSizeCode(this string value)
    {
      if (value != null)
      {
        value = value.Trim();
      }

      return string.IsNullOrEmpty(value) ? "STD" : value;
    }

    public static string ToShortYear(this string value)
    {
      if (value.Length < 4)
      {
        return value;
      }
      else
      {
        return value.Substring(2, 2);
      }
    }

    public static string CreatePassword(int passwordLength)
    {
      string allowedChars = "abcdefghijkmnopqrstuvwxyz0123456789";

      Byte[] randomBytes = new Byte[passwordLength];

      char[] chars = new char[passwordLength];

      int allowedCharCount = allowedChars.Length;

      Random randomObj = new Random();

      for (int i = 0; i < passwordLength; i++)
      {
        randomObj.NextBytes(randomBytes);
        chars[i] = allowedChars[(int)randomBytes[i] % allowedCharCount];
      }

      return new string(chars);

    }
  }
}
