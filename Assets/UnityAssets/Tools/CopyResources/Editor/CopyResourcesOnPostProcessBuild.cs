namespace Assets.CopyResources.Editor
{
    using System.Collections;
    using System.Diagnostics;
    using System.IO;

    using UnityEditor;
    using UnityEditor.Callbacks;

    public class CopyResourcesOnPostProcessBuild
    {
        private const string ScriptFileName = "CopyResources.ps1";

        private const string ProcessName = "PowerShell.exe";

        [PostProcessBuild(0)]
        public static void OnPostProcessBuild(BuildTarget target, string pathToBuildProject)
        {
            UnityEngine.Debug.Log("Copying resources");

            var files = Directory.GetFiles(
                Directory.GetCurrentDirectory(),
                ScriptFileName,
                SearchOption.AllDirectories);

            if (files.Length != 1)
            {
                UnityEngine.Debug.LogError(string.Format("Cannot find script {0}", ScriptFileName));
                return;
            }

            string scriptFilePath = files[0];

            ArrayList argumentListProcess = new ArrayList
            {
                "-NoProfile",
                "-ExecutionPolicy",
                "Bypass",
                "-File",
                scriptFilePath,
                Directory.GetCurrentDirectory(),
                pathToBuildProject
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
                WorkingDirectory = Path.GetDirectoryName(scriptFilePath),
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
                    UnityEngine.Debug.Log("Resources copied successfully!");
                }
                else if (process.ExitCode == 2)
                {
                    UnityEngine.Debug.LogWarning("Some files were not copied to the destination folder. View log!");
                }
                else
                {
                    UnityEngine.Debug.LogError(string.Format("Fail execution of script {0}", ScriptFileName));
                }
            }
            else
            {
                UnityEngine.Debug.LogError(string.Format("Cannot create process {0}", ProcessName));
            }
        }
    }
}
