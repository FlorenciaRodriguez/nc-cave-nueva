namespace Assets.UnityAssets.Editor.Platforms
{
  using System;

  using System.Collections.Generic;

  using System.IO;

  using Assets.UnityAssets.Editor.Platforms.Hooks;

  using Assets.UnityAssets.Editor.Platforms.Hooks.Android;

  using Assets.UnityAssets.Editor.Platforms.Hooks.Windows64;

  using UnityEditor;

  using UnityEngine;

  /// <summary>
  /// Supported build platforms
  /// </summary>
  public enum PlatformsEnum
  {
    /// <summary>
    /// Android APK
    /// </summary>
    Android,

    /// <summary>
    /// Standalone Windows 64
    /// </summary>
    Windows64
  }

  public class PlatformManager : EditorWindow
  {
    private static PlatformScriptableObject platform;

    /// <summary>
    /// Add menu "PlatformManager" to the Window menu
    /// </summary>
    [MenuItem("Tools/Platform Manager")]
    public static void Init()
    {
      GetWindow<PlatformManager>().Show();
    }

    public static bool BuildAll()
    {
      VerifyAsset();

      if (platform.BuildAndroid)
      {
        var hookTypes = BuildHelper.GetHooks<AndroidBaseHook>();
        var result = Build(BuildTarget.Android, hookTypes, PlatformsEnum.Android.ToString());
        if (!result)
        {
          return false;
        }
      }

      if (platform.BuildWindows64)
      {
        var hookTypes = BuildHelper.GetHooks<Windows64BaseHook>();
        var result = Build(BuildTarget.StandaloneWindows64, hookTypes, PlatformsEnum.Windows64.ToString());
        if (!result)
        {
          return false;
        }
      }

      return true;
    }

    public static bool Build(BuildTarget buildTarget, List<Type> hookTypes, string targetName, bool releaseTarget = true)
    {
      var buildFolder = Path.Combine(Directory.GetCurrentDirectory(), "Build");
      var logFilePath = Path.Combine(buildFolder, "Build.log");

      if (!Directory.Exists(buildFolder))
      {
        Directory.CreateDirectory(buildFolder);
      }

#if UNITY_2017
      var logFile = File.CreateText(logFilePath);
      logFile.WriteLine("BUILD REPORT");

      Application.LogCallback logCallback = (condition, trace, type) => logFile.WriteLine($"[{type}]: {condition}");
      Application.logMessageReceived += logCallback;
#endif

      Debug.Log(string.Format("Building {0}", targetName));

      List<IBaseHook> hooks = new List<IBaseHook>();
      hookTypes.ForEach(
        hookType =>
        {
          // Create Hook
          var hook = Activator.CreateInstance(hookType) as IBaseHook;
          if (hook != null)
          {
            hooks.Add(hook);

            // Execute PreBuild Hook
            Debug.Log("Executing PreBuild Hook: " + hookType);
            hook.PreBuild();
          }
        });

      // Get scenes to compile
      var sceneList = BuildHelper.GetSceneList().ToArray();

      // File name
      string folderPath = Path.Combine(Directory.GetCurrentDirectory(), string.Format("Build/{0}", targetName));
      string binaryExtension = BuildHelper.GetBinaryExtension(buildTarget);
      string filePath = Path.Combine(folderPath, string.Format("{0}.{1}", PlayerSettings.productName, binaryExtension));

      BuildOptions buildOptionsDebug = BuildOptions.Development | BuildOptions.AllowDebugging | BuildOptions.StrictMode;
      BuildOptions buildOptionsRelease = BuildOptions.StrictMode;
      BuildOptions buildOptions = releaseTarget ? buildOptionsRelease : buildOptionsDebug;

      // Realizar el Build
      var result = BuildPipeline.BuildPlayer(
        sceneList,
        filePath,
        buildTarget,
        buildOptions);

      // Execute PostBuild Hook
      hooks.ForEach(
        hook =>
        {
          Debug.Log("Executing PostBuild Hook: " + hook.GetType());
          hook.PostBuild();
        });

      bool succeeded;

#if UNITY_2017
      if (string.IsNullOrEmpty(result))
      {
        Debug.Log(string.Format("Build {0}: Succeeded!", targetName));
        succeeded = true;
      }
      else
      {
        Debug.LogError(string.Format("Build {0}: Failed!", targetName));
        succeeded = false;
      }

      logFile.Close();
      Application.logMessageReceived -= logCallback;

#elif UNITY_2018 || UNITY_2018_1_OR_NEWER
      LogBuildReport(result, logFilePath);

      if (result.summary.result == UnityEditor.Build.Reporting.BuildResult.Succeeded)
      {
        Debug.Log(string.Format("Build {0}: Succeeded!", targetName));
        succeeded = true;
      }
      else
      {
        Debug.LogError(string.Format("Build {0}: Failed!", targetName));
        succeeded = false;
      }
#endif
      return succeeded;
    }

    protected void OnEnable()
    {
      this.titleContent.text = "Platforms";
    }

    // ReSharper disable once InconsistentNaming
    protected void OnGUI()
    {
      VerifyAsset();

      platform.BuildAndroid = EditorGUILayout.BeginToggleGroup("Android", platform.BuildAndroid);

      GuiVerifyJdkPath();
      GuiVerifyAndroidSdkRoot();
      GuiVerifyApplicationIdentifier();

      if (GUILayout.Button("Build"))
      {
        var hookTypes = BuildHelper.GetHooks<AndroidBaseHook>();
        Build(BuildTarget.Android, hookTypes, "Android");
      }

      EditorGUILayout.EndToggleGroup();

      platform.BuildWindows64 = EditorGUILayout.BeginToggleGroup("Windows64", platform.BuildWindows64);

      if (GUILayout.Button("Build"))
      {
        var hookTypes = BuildHelper.GetHooks<Windows64BaseHook>();
        Build(BuildTarget.StandaloneWindows64, hookTypes, "Windows64");
      }

      EditorGUILayout.EndToggleGroup();

      if (GUILayout.Button("Build All!"))
      {
        BuildAll();
      }

      if (GUI.changed)
      {
        EditorUtility.SetDirty(platform);
      }
    }

    private static void VerifyAsset()
    {
      if (!File.Exists("Assets/PlatformManager.asset"))
      {
        // ReSharper disable once ArrangeStaticMemberQualifier
        platform = ScriptableObject.CreateInstance<PlatformScriptableObject>();
        AssetDatabase.CreateAsset(platform, "Assets/PlatformManager.asset");
        AssetDatabase.SaveAssets();
      }

      if (platform == null)
      {
        platform = AssetDatabase.LoadAssetAtPath<PlatformScriptableObject>("Assets/PlatformManager.asset");
      }
    }

    private static void GuiVerifyJdkPath()
    {
      if (!BuildHelper.IsJdkPathSet())
      {
        EditorGUILayout.HelpBox("JDK not configured!", MessageType.Error);
      }
      else if (!BuildHelper.IsJdkPathValid())
      {
        EditorGUILayout.HelpBox("JDK directory not found! " + BuildHelper.GetJdkPath(), MessageType.Error);
      }
      else
      {
        EditorGUILayout.HelpBox("JDK path: " + BuildHelper.GetJdkPath(), MessageType.None);
      }
    }

    private static void GuiVerifyAndroidSdkRoot()
    {
      if (!BuildHelper.IsAndroidSdkRootSet())
      {
        EditorGUILayout.HelpBox("Android SDK not configured!", MessageType.Error);
      }
      else if (!BuildHelper.IsAndroidSdkRootValid())
      {
        EditorGUILayout.HelpBox(
          "Android SDK directory not found! " + BuildHelper.GetAndroidSdkRoot(),
          MessageType.Error);
      }
      else
      {
        EditorGUILayout.HelpBox("Android SDK path: " + BuildHelper.GetAndroidSdkRoot(), MessageType.None);
      }
    }

    private static void GuiVerifyApplicationIdentifier()
    {
      MessageType messageType = BuildHelper.IsApplicationIdentifierValid() ? MessageType.Error : MessageType.None;
      EditorGUILayout.HelpBox("Application Identifier: " + BuildHelper.GetApplicationIdentifier(), messageType);
    }

#if UNITY_2018 || UNITY_2018_1_OR_NEWER
    private static void LogBuildReport(UnityEditor.Build.Reporting.BuildReport buildReport, string filePath)
    {
      var logFile = File.CreateText(filePath);

      logFile.WriteLine("BUILD REPORT");
      logFile.WriteLine($"Result: {buildReport.summary.result}");
      logFile.WriteLine($"Total warnings: {buildReport.summary.totalWarnings}");
      logFile.WriteLine($"Total errors: {buildReport.summary.totalErrors}");

      logFile.WriteLine();
      logFile.WriteLine("STEPS");

      for (var i = 0; i < buildReport.steps.Length; i++)
      {
        var buildStep = buildReport.steps[i];
        logFile.WriteLine($"Step {i + 1}: {buildStep.name} ({buildStep.duration.TotalMinutes:00}:{buildStep.duration.Seconds:00}.{buildStep.duration.Milliseconds:000})");

        foreach (var buildStepMessage in buildStep.messages)
        {
          logFile.WriteLine($"- [{buildStepMessage.type}]: {buildStepMessage.content}");
        }
      }

      logFile.Close();
    }
#endif
  }
}
