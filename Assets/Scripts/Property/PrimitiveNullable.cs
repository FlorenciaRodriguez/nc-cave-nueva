namespace Assets.Scripts.Property
{
  using System;

  [Serializable]
  public struct PrimitiveNullable<T> where T : unmanaged
  {
    public bool HasValue;

    public T Value;
  }

  public static class PrimitiveNullabeExtensionHelper
  {
    public static float? Convert(this PrimitiveNullable<float> primitiveNullable)
    {
      if (primitiveNullable.HasValue)
      {
        return primitiveNullable.Value;
      }

      return null;
    }

    public static int? Convert(this PrimitiveNullable<int> primitiveNullable)
    {
      if (primitiveNullable.HasValue)
      {
        return primitiveNullable.Value;
      }

      return null;
    }

    public static bool? Convert(this PrimitiveNullable<bool> primitiveNullable)
    {
      if (primitiveNullable.HasValue)
      {
        return primitiveNullable.Value;
      }

      return null;
    }
  }
}