namespace Assets.UnityAssets.Editor
{
  using System.Collections;
  using System.Diagnostics;
  using System.IO;
  using System.Xml;

  using UnityEditor;

  public class BuildFrameworkScript
  {
    private const string SettingsFileName = "BuildFrameworkSettings.xml";

    private const string ProcessName = "PowerShell.exe";

    private const string BuildPsScriptNode = "Settings/BuildPSScript";

    private const string PathPsScriptAttribute = "Path";

    [MenuItem("Tools/Build Framework")]
    public static void BuildFramework()
    {
      UnityEngine.Debug.Log("Building framework...");

      var files = Directory.GetFiles(Directory.GetCurrentDirectory(), SettingsFileName, SearchOption.AllDirectories);
      if (files.Length != 1)
      {
        UnityEngine.Debug.LogError(string.Format("Cannot find xml file {0}", SettingsFileName));
        return;
      }

      string fullPathSettings = files[0];
      string fullPathPsScript = null;

      XmlDocument xmlDocument = new XmlDocument();
      xmlDocument.Load(fullPathSettings);
      var selectSingleNode = xmlDocument.SelectSingleNode(BuildPsScriptNode);
      if (selectSingleNode != null && selectSingleNode.Attributes != null)
      {
        fullPathPsScript = selectSingleNode.Attributes[PathPsScriptAttribute].InnerXml;
      }
      else
      {
        UnityEngine.Debug.LogError(string.Format("Missing attribute {0} in node {1}", PathPsScriptAttribute, BuildPsScriptNode));
        return;
      }

      // ReSharper disable AssignNullToNotNullAttribute
      fullPathPsScript = Path.GetFullPath(Path.Combine(Path.GetDirectoryName(fullPathSettings), fullPathPsScript));

      ArrayList argumentListProcess = new ArrayList
      {
        "-NoProfile",
        "-ExecutionPolicy",
        "Bypass",
        "-File",
        fullPathPsScript,
        "-GroupName",
        "Visual",
        "-FullPathSettings",
        fullPathSettings
      };

      string strArgumentListProcess = string.Empty;
      var enumerator = argumentListProcess.GetEnumerator();
      while (enumerator.MoveNext())
      {
        string arg = enumerator.Current as string;

        Debug.Assert(arg != null, "Argument is null");

        if (arg.Contains(" "))
        {
          arg = "\"" + arg + "\"";
        }

        strArgumentListProcess += arg + " ";
      }

      ProcessStartInfo processStartInfo = new ProcessStartInfo(ProcessName, strArgumentListProcess)
      {
        // ReSharper disable AssignNullToNotNullAttribute
        WorkingDirectory = Path.GetDirectoryName(fullPathPsScript),
        // ReSharper restore AssignNullToNotNullAttribute
        UseShellExecute = false,
        CreateNoWindow = true
      };

      var process = Process.Start(processStartInfo);
      if (process != null)
      {
        process.WaitForExit();
        if (process.ExitCode == 0)
        {
          UnityEngine.Debug.Log("Build succeeded!");
          AssetDatabase.Refresh();
        }
        else
        {
          UnityEngine.Debug.LogError("Build failed!");
        }
      }
      else
      {
        UnityEngine.Debug.LogError(string.Format("Cannot create process {0}", ProcessName));
      }
    }
  }
}
