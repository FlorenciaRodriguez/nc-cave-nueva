namespace Assets.UnityAssets.Tools.Utilities.Editor
{
  using UnityEditor;

  using UnityEngine;

  public static class AssetHideFlags
  {
    [MenuItem("CONTEXT/Object/Reset Hide Flags", false, -100)]
    public static void MenuResetHideFlags(MenuCommand command)
    {
      command.context.hideFlags = HideFlags.None;
      EditorUtility.SetDirty(command.context);
    }

    [MenuItem("CONTEXT/Object/Don't Save In Build", false, -100)]
    public static void MenuApplyDontSaveInBuild(MenuCommand command)
    {
      var dontSaveInBuild = (command.context.hideFlags & HideFlags.DontSaveInBuild) != 0;

      if (!dontSaveInBuild)
      {
        command.context.hideFlags |= HideFlags.DontSaveInBuild;
        EditorUtility.SetDirty(command.context);
      }
    }
  }
}
