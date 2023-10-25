namespace Assets.Scripts.Helpers
{
  using System;

  using MediaLab.Common.Serializables;

  using UnityEngine;

  public static class FastEqualityHelper
  {
    public static bool Equals(double? value1, double? value2)
    {
      if (value1.HasValue)
      {
        if (value2.HasValue)
        {
          return Math.Abs(value1.Value - value2.Value) < Mathf.Epsilon;
        }

        return false;
      }

      return !value2.HasValue;
    }

    public static bool Equals(float? value1, float? value2)
    {
      if (value1.HasValue)
      {
        if (value2.HasValue)
        {
          return Math.Abs(value1.Value - value2.Value) < Mathf.Epsilon;
        }

        return false;
      }

      return !value2.HasValue;
    }

    public static bool Equals(int? value1, int? value2)
    {
      if (value1.HasValue)
      {
        if (value2.HasValue)
        {
          return value1.Value == value2.Value;
        }

        return false;
      }

      return !value2.HasValue;
    }

    public static bool Equals(bool? value1, bool? value2)
    {
      if (value1.HasValue)
      {
        if (value2.HasValue)
        {
          return value1.Value == value2.Value;
        }

        return false;
      }

      return !value2.HasValue;
    }

    public static bool Equals(SerializableVector3 value1, SerializableVector3 value2)
    {
      if (value1 != null)
      {
        if (value2 != null)
        {
          return Equals(value1.Vector3, value2.Vector3);
        }

        return false;
      }

      return value2 == null;
    }

    public static bool Equals(Vector3 value1, Vector3 value2)
    {
      return Math.Abs(value1.x - value2.x) < Mathf.Epsilon && Math.Abs(value1.y - value2.y) < Mathf.Epsilon && Math.Abs(value1.z - value2.z) < Mathf.Epsilon;
    }
  }
}
