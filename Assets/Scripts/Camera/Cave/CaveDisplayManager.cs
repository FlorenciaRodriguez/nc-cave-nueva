namespace Assets.Scripts.Camera.Cave
{
  using System;
  using System.IO;
  using System.Linq;

  using Assets.Scripts.Camera.Cave.Serializable;
  using Assets.Scripts.Helpers;

  using UnityEngine;
  using UnityEngine.Events;

  public static class CaveDisplayManager
  {
    private const string FolderName = "Cave";

    private const string FileName = "CaveDisplaySettings.xml";

    private static readonly string StreamingAssetsFolderPath = Path.GetFullPath(Path.Combine(Application.streamingAssetsPath, FolderName, FileName));

    private static readonly string LocalApplicationDataFolderPath =
      Path.GetFullPath(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), Application.companyName, Application.productName, FolderName, FileName));

    private static CaveDisplaySettings caveDisplaySettings;

    public static event UnityAction Updated;

    private static CaveDisplaySettings CaveDisplaySettings => caveDisplaySettings ?? (caveDisplaySettings = LoadCaveDisplaySettings());

    public static int GetDisplay(CaveCameraEnum caveCamera)
    {
      return GetProfile(caveCamera).TargetDisplay;
    }

    public static void SetDisplay(CaveCameraEnum caveCamera, int display)
    {
      var targetProfile = GetProfile(caveCamera);
      var otherProfile = GetProfile(display);

      if (targetProfile.CaveCamera != otherProfile.CaveCamera)
      {
        var caveDisplay = targetProfile.TargetDisplay;
        targetProfile.TargetDisplay = display;
        otherProfile.TargetDisplay = caveDisplay;

        Save();
        Updated?.Invoke();
      }
    }

    public static void Save()
    {
      if (caveDisplaySettings != null)
      {
        SaveCaveDisplaySettings(caveDisplaySettings, LocalApplicationDataFolderPath);
        SaveCaveDisplaySettings(caveDisplaySettings, StreamingAssetsFolderPath);
      }
    }

    private static CaveDisplaySettings LoadCaveDisplaySettings()
    {
      CaveDisplaySettings settings;
      try
      {
        settings = LoadCaveDisplaySettingsFromFile(LocalApplicationDataFolderPath);
      }
      catch
      {
        try
        {
          settings = LoadCaveDisplaySettingsFromFile(StreamingAssetsFolderPath);
        }
        catch
        {
          settings = CreateDefaultCaveDisplaySettings();
        }
      }

      ValidateCaveDisplaySettings(settings);

      return settings;
    }

    private static CaveDisplaySettings LoadCaveDisplaySettingsFromFile(string filePath)
    {
      if (!File.Exists(filePath))
      {
        throw new FileNotFoundException($"Cave display settings file path '{filePath}' not found", filePath);
      }

      try
      {
        return XMLSerializationHelper.DeserializeFromXmlFile<CaveDisplaySettings>(filePath);
      }
      catch (Exception exception)
      {
        throw new InvalidDataException(
          $"Invalid data '{nameof(CaveDisplaySettings)}' from file '{filePath}': {exception.GetBaseException().Message}",
          exception);
      }
    }

    private static CaveDisplaySettings CreateDefaultCaveDisplaySettings()
    {
      var values = Enum.GetValues(typeof(CaveCameraEnum));

      var settings = new CaveDisplaySettings { CaveDisplayProfile = new CaveDisplayProfile[values.Length] };

      foreach (CaveCameraEnum value in values)
      {
        var index = (int)value;
        settings.CaveDisplayProfile[index] = new CaveDisplayProfile { CaveCamera = value, TargetDisplay = index };
      }

      return settings;
    }

    private static void ValidateCaveDisplaySettings(CaveDisplaySettings settings)
    {
      var values = Enum.GetValues(typeof(CaveCameraEnum));

      if (settings.CaveDisplayProfile == null)
      {
        settings.CaveDisplayProfile = new CaveDisplayProfile[values.Length];
      }

      var profiles = settings.CaveDisplayProfile;

      foreach (CaveCameraEnum value in values)
      {
        if (profiles.All(profile => profile.CaveCamera != value))
        {
          Array.Resize(ref profiles, settings.CaveDisplayProfile.Length + 1);

          profiles[profiles.Length - 1] = new CaveDisplayProfile { CaveCamera = value, TargetDisplay = (int)value };
        }
      }

      settings.CaveDisplayProfile = profiles;
    }

    private static void SaveCaveDisplaySettings(CaveDisplaySettings settings, string filePath)
    {
      try
      {
        XMLSerializationHelper.SerializeToXmlFile(settings, filePath);
      }
      catch
      {
        // ignored
      }
    }

    private static CaveDisplayProfile GetProfile(CaveCameraEnum caveCamera)
    {
      foreach (var caveDisplayProfile in CaveDisplaySettings.CaveDisplayProfile)
      {
        if (caveDisplayProfile.CaveCamera == caveCamera)
        {
          return caveDisplayProfile;
        }
      }

      return null;
    }

    private static CaveDisplayProfile GetProfile(int targetDisplay)
    {
      foreach (var caveDisplayProfile in CaveDisplaySettings.CaveDisplayProfile)
      {
        if (caveDisplayProfile.TargetDisplay == targetDisplay)
        {
          return caveDisplayProfile;
        }
      }

      return null;
    }
  }
}
