namespace Assets.UnityAssets.Editor
{
  using System.Linq;

  using UnityEditor;
  using UnityEditor.SceneManagement;

  using UnityEngine;
  using UnityEngine.SceneManagement;

  public class ProjectConfigurator
  {
    [MenuItem("Tools/Configure Project")]
    public static void ButtonConfigureProject()
    {
      ConfigureProject();
    }

    public static void ConfigureProject()
    {
      Debug.Log("Configure Project");

      // Asset Serialization Mode: Force Text
      EditorSettings.serializationMode = SerializationMode.ForceText;

      // Api Compatibility Level: .NET 4.6
      PlayerSettings.SetApiCompatibilityLevel(BuildTargetGroup.Android, ApiCompatibilityLevel.NET_4_6);
      PlayerSettings.SetApiCompatibilityLevel(BuildTargetGroup.Standalone, ApiCompatibilityLevel.NET_4_6);

      PlayerSettings.companyName = "MediaLab";
      PlayerSettings.SetApplicationIdentifier(BuildTargetGroup.Standalone, string.Format("com.{0}", PlayerSettings.productName));
      PlayerSettings.SetApplicationIdentifier(BuildTargetGroup.Android, string.Format("com.{0}", PlayerSettings.productName));

      // Save "Startup.scene" Scene in "_Scenes" folder
      EditorSceneManager.SaveScene(SceneManager.GetActiveScene(), "Assets/_Scenes/Startup.unity", false);
      
      // Build Settings: Add Startup scene
      var startupBuildSettingsScene = new EditorBuildSettingsScene("Assets/_Scenes/Startup.unity", true);
      var editorBuildSettingsScenes = EditorBuildSettings.scenes.ToList();
      editorBuildSettingsScenes.Add(startupBuildSettingsScene);
      EditorBuildSettings.scenes = editorBuildSettingsScenes.ToArray();

      // Build Settings Architecture: x86_64 
      EditorUserBuildSettings.SwitchActiveBuildTarget(BuildTargetGroup.Standalone, BuildTarget.StandaloneWindows64);

      AssetDatabase.SaveAssets();
    }
  }
}
