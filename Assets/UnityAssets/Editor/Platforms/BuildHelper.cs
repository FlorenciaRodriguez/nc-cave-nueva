namespace Assets.UnityAssets.Editor.Platforms
{
  using System;
  using System.Collections.Generic;
  using System.IO;
  using System.Linq;
  using System.Reflection;

  using Microsoft.Win32;

  using UnityEditor;

  using UnityEngine;

  public class BuildHelper : MonoBehaviour
  {
    public static string GetAndroidSdkRoot()
    {
      return EditorPrefs.GetString("AndroidSdkRoot");
    }

    public static bool IsAndroidSdkRootSet()
    {
      return EditorPrefs.HasKey("AndroidSdkRoot");
    }

    public static bool IsAndroidSdkRootValid()
    {
      return Directory.Exists(EditorPrefs.GetString("AndroidSdkRoot"));
    }

    public static string GetJdkPath()
    {
      return EditorPrefs.GetString("JdkPath");
    }

    public static bool IsJdkPathSet()
    {
      return EditorPrefs.HasKey("JdkPath");
    }

    public static bool IsJdkPathValid()
    {
      return Directory.Exists(EditorPrefs.GetString("JdkPath"));
    }

    public static string GetApplicationIdentifier()
    {
      return PlayerSettings.applicationIdentifier;
    }

    public static bool IsApplicationIdentifierValid()
    {
      return PlayerSettings.applicationIdentifier.Equals("com.Company.ProductName");
    }

    public static List<Type> GetHooks<T>()
    {
      return Assembly.GetExecutingAssembly().GetTypes()
        .Where(type => typeof(T).IsAssignableFrom(type) && typeof(T) != type)
        .ToList();
    }

    public static List<string> GetSceneList()
    {
      List<string> sceneList = new List<string>();

      foreach (EditorBuildSettingsScene scene in EditorBuildSettings.scenes)
      {
        if (scene.enabled)
        {
          sceneList.Add(scene.path);
        }
      }

      return sceneList;
    }

    public static string GetBinaryExtension(BuildTarget buildTarget)
    {
      switch (buildTarget)
      {
        case BuildTarget.Android:
          return "apk";
        case BuildTarget.StandaloneWindows:
        case BuildTarget.StandaloneWindows64:
          return "exe";
      }

      return string.Empty;
    }

    public static string GetJavaInstallationPath()
    {
      try
      {
        var environmentPath = Environment.GetEnvironmentVariable("JAVA_HOME");
        if (!string.IsNullOrEmpty(environmentPath))
        {
          Debug.Log("Using JAVE_HOME environment variable");
          return environmentPath;
        }

        var javaKey = "SOFTWARE\\JavaSoft\\Java Runtime Environment\\";
        using (var rk = Registry.LocalMachine.OpenSubKey(javaKey))
        {
          var currentVersion = rk.GetValue("CurrentVersion").ToString();
          using (var key = rk.OpenSubKey(currentVersion))
          {
            Debug.Log("Using registry JDK path");
            return key.GetValue("JavaHome").ToString();
          }
        }
      }
      catch (Exception e)
      {
        Debug.LogWarning("JDK path could not be detected!");
        Debug.LogWarning(e.StackTrace);
        return null;
      }
    }

    public static void FixAndroidSdkRoot()
    {
      string defaultSdkRoot = "C:/Android/SDK";
      Debug.Log(string.Format("Setting Android SDK Root: {0}", defaultSdkRoot));
      EditorPrefs.SetString("AndroidSdkRoot", defaultSdkRoot);
    }

    public static void FixJdkPath()
    {
      var jdkPath = GetJavaInstallationPath();
      Debug.Log(string.Format("Setting JDK Path: {0}", jdkPath));
      EditorPrefs.SetString("JdkPath", jdkPath);
    }
  }
}
