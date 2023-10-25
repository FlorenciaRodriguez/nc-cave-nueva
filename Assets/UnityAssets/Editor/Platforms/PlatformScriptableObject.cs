namespace Assets.UnityAssets.Editor.Platforms
{
  using System;
  using System.Diagnostics.CodeAnalysis;

  using UnityEngine;

  [SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1401:FieldsMustBePrivate", Justification = "Reviewed. Suppression is OK here.")]
  [Serializable]
  public class PlatformScriptableObject : ScriptableObject
  {
    public bool BuildAndroid;

    public bool BuildWindows64 = true;

    public bool BuildOculus;
  }
}
