// ReSharper disable InconsistentNaming
namespace Assets.Scripts.Helpers
{
  using System;
  using System.IO;

  using MediaLab.Simpa.Settings.Properties;

  using UnityEngine;

  public class ApplicationHelper
  {
    // Application Mode
    private const string StrModeSandbox = "Sandbox";

    // Camera Mode
    private const string StrModeCave = "Cave";

    public enum ApplicationModeEnum
    {
      Production,
      Sandbox
    }

    public enum ApplicationCameraModeEnum
    {
      Default,
      Cave
    }

    public enum AppTypeEnum
    {
      LocationTour
    }

    public static ApplicationModeEnum ApplicationMode
    {
      get
      {
        if (Settings.Default.Mode.Equals(StrModeSandbox, StringComparison.OrdinalIgnoreCase))
        {
          return ApplicationModeEnum.Sandbox;
        }

        return ApplicationModeEnum.Production;
      }
    }

    public static ApplicationCameraModeEnum ApplicationCameraMode
    {
      get
      {
        if (Settings.Default.CameraMode.Equals(StrModeCave, StringComparison.OrdinalIgnoreCase))
        {
          return ApplicationCameraModeEnum.Cave;
        }

        return ApplicationCameraModeEnum.Default;
      }
    }

    public static void Exit()
    {
#if UNITY_EDITOR
      UnityEditor.EditorApplication.isPlaying = false;
#else
      UnityEngine.Application.Quit();
#endif
    }
  }
}
