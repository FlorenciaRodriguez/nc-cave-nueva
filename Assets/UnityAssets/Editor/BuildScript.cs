#if UNITY_2017
using System.Linq;
#endif

using Assets.UnityAssets.Editor.Platforms;

using UnityEditor;

// ReSharper disable once CheckNamespace
public class BuildScript
{
  public static void PerformBuild()
  {
    var result = PlatformManager.BuildAll();

#if UNITY_2017
    if (System.Environment.GetCommandLineArgs().Any(arg => string.Equals(arg, "-batchmode", System.StringComparison.OrdinalIgnoreCase)))
    {
      EditorApplication.Exit(result ? 0 : 1);
    }
#elif UNITY_2018 || UNITY_2018_1_OR_NEWER
    if (UnityEngine.Application.isBatchMode)
    {
      EditorApplication.Exit(result ? 0 : 1);
    }
#endif
  }

  [MenuItem("Tools/Build Project")]
  protected static void BuildProject()
  {
    PerformBuild();
  }
}