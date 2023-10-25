namespace Assets.UI.Common.Scripts.Helpers
{
  using System;
  using System.Diagnostics.CodeAnalysis;
  using System.Resources;

  public static class ResexsHelper
  {
    public const string DefaultKey = "Default";

    [SuppressMessage("ReSharper", "InconsistentNaming", Justification = "Reviewed. Suppression is OK here.")]
    public enum ResexTypeEnum
    {
      UI
    }

    [SuppressMessage("ReSharper", "InconsistentNaming", Justification = "Reviewed. Suppression is OK here.")]
    public enum UIResourceNameEnum
    {
      Default,
      IconExerciseStatePaused,
      IconExerciseStateWaiting,
      LabelExerciseStatePaused,
      LabelExerciseStateWaiting,
      LabelHelpInput,
      LabelSandboxHelpInput,
      LabelLocationTourHelpInput,
      LabeEquipmentRecognitionHelpInput,
      LabelHelpMenuSelectItem,
      LabelHelpMenuNavigate,
      LabelHelpMenuClose,
      LabelImageViewerLoadError,
      LabelPdfViewerLoadError,
      LabelMessageBoxOk,
      LabelMessageBoxCancel,
      LabelMessageBoxYes,
      LabelMessageBoxNo,
      LabelButtonSkipVideo,
      LabelButtonOpenMultimediaMenu,
      LabelHelpApplicationClose,
      LabelHelpInputXbox,
      LabelHelpFullScreen,
      LabelHelpPdfAdjustPage,
      LabelHelpPdfScroll,
      LabelHelpPdfZoomIn,
      LabelHelpPdfZoomOut,
      LabelHelpVideoAdvanceRewind,
      LabelHelpVideoPlayPause,
      LabelHelpVideoStop,
      LabelTitleHelpXboxVideo,
      LabelTitleHelpXboxImage,
      LabelTitleHelpXboxPdf,
      LabelTitleHelpXboxPointOfInterest,
      LabelHelpXboxLookAt,
      LabelTitleHelpXboxWalk,
      LabelTitleHelpXboxDrone,
      LabelHelpXboxWalk,
      LabelHelpXboxRun,
      LabelHelpXboxToSpeed,
      LabelHelpXboxMove,
      LabelHelpXboxUpDown,
      LabelHelpVideoUpDownVolume,
      LabelOperationHelpInput,
      IconProcessStateStarted,
      IconProcessStateFailed,
      IconProcessStateCompleted,
      LabelMessageProcessStateStarted,
      LabelMessageProcessStateFailed,
      LabelMessageProcessStateCompleted,
      LabelHelpXboxTopDriveUp,
      LabelHelpXboxIncreaseHookLoad,
      LabelHelpXboxDecreaseHookLoad,
      LabelHelpXboxIncreaseRpm,
      LabelHelpXboxDecreaseRpm,
      LabelTitleHelpXboxOperation,
      LabelHelpXboxTopDriveDown,
      LabelHelpXboxElevatorLeft,
      LabelHelpXboxElevatorRight,
      LabelHelpXboxLookAtUpDown,
      LabelHelpXboxNextPointOfInterest,
      LabelHelpXboxPreviousPointOfInterest,
      LabelLocationTourXRHelpInput,
      LabelButtonNext,
      LabelButtonClose,
      LabelHelpXRUITitle,
      LabelHelpXRUIDescription,
      LabelHelpXRUICanvas,
      LabelHelpXRUIController,
      LabelHelpXRNavigationMoveTitle,
      LabelHelpXRNavigationMoveDescription,
      LabelHelpXRNavigationMoveTranslate,
      LabelHelpXRNavigationMoveTeleport,
      LabelHelpXRNavigationRotationTitle,
      LabelHelpXRNavigationRotationDescription,
      LabelHelpXRNavigationRotation45,
      LabelHelpXRNavigationRotation180,
      LabelHelpXRNavigationLadderTitle,
      LabelHelpXRNavigationLadderDescription,
      LabelHelpXRNavigationLadderUp,
      LabelHelpXRNavigationLadderDown
    }

    public static string GetResourceStringValue(string key, ResexTypeEnum resexTypeEnum)
    {
      var resourceManager = GetResourceManager(resexTypeEnum);
      return resourceManager.GetString(key) ?? resourceManager.GetString(DefaultKey);
    }

    public static string GetDefault(ResexTypeEnum textTypeEnum)
    {
      return GetResourceManager(textTypeEnum).GetString(DefaultKey);
    }

    private static ResourceManager GetResourceManager(ResexTypeEnum textTypeEnum)
    {
      switch (textTypeEnum)
      {
        case ResexTypeEnum.UI:
          return MediaLab.Simpa.Settings.Resources.Resexs.UI.ResourceManager;
        default:
          throw new ArgumentOutOfRangeException(nameof(textTypeEnum), textTypeEnum, null);
      }
    }
  }
}