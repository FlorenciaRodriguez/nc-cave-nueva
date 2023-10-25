namespace Assets.UI.Common.Scripts.Helpers
{
  using System.Collections.Generic;

  using MediaLab.Common;

  using UnityEngine;

  public static class IconHelper
  {
    private static readonly Dictionary<Constants.EventTypeEnum, Sprite> Icons = new Dictionary<Constants.EventTypeEnum, Sprite>();

    public static Sprite GetIcon(Constants.EventTypeEnum iconType)
    {
      Sprite icon;
      if (Icons.TryGetValue(iconType, out icon))
      {
        return icon;
      }

      icon = Resources.Load<Sprite>(iconType.ToString());
      if (icon != null)
      {
        Icons.Add(iconType, icon);
      }
      else
      {
        Debug.LogWarning($"Icon resource type '{iconType}' was not found");
      }

      return icon;
    }
  }
}
