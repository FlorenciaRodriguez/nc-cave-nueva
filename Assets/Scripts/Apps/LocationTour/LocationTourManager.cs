#pragma warning disable 649
namespace Assets.Scripts.Apps.LocationTour
{
  using System.Linq;

  using Assets.Scripts.Apps.Receivers;
  using Assets.Scripts.Communication;
  using Assets.Scripts.Helpers;

  using MediaLab.Adic.Framework.Attributes;
  using MediaLab.Common.Serializables;
  using MediaLab.NetworkEntities.Simpa;
  using MediaLab.NetworkEntities.Simpa.Assets;
  using MediaLab.NetworkEntities.Simpa.Configuration;
  using MediaLab.NetworkEntities.Simpa.Exercises;

  using UnityEngine;
  using UnityEngine.Events;

  public class LocationTourManager : BaseAppManager
  {
    private readonly MomentOfTheDayEntity currentMomentOfTheDayEntity =
      new MomentOfTheDayEntity { MomentOfTheDay = Constants.MomentsOfTheDayEnum.Sunrise };

    private readonly LocationEntity currentLocationEntity =
      new LocationEntity { ScreenPoint = new SerializableVector2(0f, 0f) };

    private readonly LocationGuyEntity currentLocationGuyEntity =
      new LocationGuyEntity { ScreenPoint = new SerializableVector2(0f, 0f) };

    [Inject]
    private CommunicationManager communicationManager;

    [Inject]
    private AppViewEntityReceiver appViewEntityReceiver;

    [Inject]
    private LocationEntityReceiver locationEntityReceiver;

    [Inject]
    private LocationGuyEntityReceiver locationGuyEntityReceiver;

    [Inject]
    private MomentOfTheDayEntityReceiver momentOfTheDayEntityReceiver;

    public event UnityAction<LocationTourManager> MomentOfTheDayChanged;

    public event UnityAction<LocationTourManager> ModeChanged;

    public enum ModeEnum
    {
      None,

      WalkMode,

      DroneMode,

      PointOfInterestMode
    }

    public ModeEnum CurrentMode { get; private set; } = ModeEnum.None;

    public Constants.MomentsOfTheDayEnum MomentOfTheDay => this.currentMomentOfTheDayEntity.MomentOfTheDay;

    public WalkModeData WalkData { get; private set; }

    public PointOfInteresetModeData PointOfInteresetData { get; private set; }

    public void HandleMomentOfTheDayEntityReceived(MomentOfTheDayEntity momentOfTheDayEntity)
    {
      if (this.currentMomentOfTheDayEntity.MomentOfTheDay != momentOfTheDayEntity.MomentOfTheDay)
      {
        this.currentMomentOfTheDayEntity.MomentOfTheDay = momentOfTheDayEntity.MomentOfTheDay;

        Debug.Log(
          $"[{nameof(LocationTourManager)}] Moment of the day changed: {this.currentMomentOfTheDayEntity.MomentOfTheDay}");

        this.MomentOfTheDayChanged?.Invoke(this);
      }
    }

    protected override void InitApp()
    {
      base.InitApp();

      this.appViewEntityReceiver.AddListener(this.HandleAppViewEntityReceived);
      this.locationEntityReceiver.AddListener(this.HandleLocationEntityReceived);
      this.locationGuyEntityReceiver.AddListener(this.HandleLocationGuyEntityReceived);
      this.momentOfTheDayEntityReceiver.AddListener(this.HandleMomentOfTheDayEntityReceived);

      this.WalkData = new WalkModeData(this);
      this.PointOfInteresetData = new PointOfInteresetModeData(this);
    }

    protected override void DestroyApp()
    {
      base.DestroyApp();

      this.appViewEntityReceiver.RemoveListener(this.HandleAppViewEntityReceived);
      this.locationEntityReceiver.RemoveListener(this.HandleLocationEntityReceived);
      this.locationGuyEntityReceiver.RemoveListener(this.HandleLocationGuyEntityReceived);
      this.momentOfTheDayEntityReceiver.RemoveListener(this.HandleMomentOfTheDayEntityReceived);
    }

    protected override void LoadApp(ExerciseEntity exerciseEntity, bool resetMode)
    {
      base.LoadApp(exerciseEntity, resetMode);

      this.CurrentMode = ModeEnum.None;

      Debug.Log($"[{nameof(LocationTourManager)}] App mode changed: {this.CurrentMode}");

      this.ModeChanged?.Invoke(this);

      this.currentMomentOfTheDayEntity.MomentOfTheDay = Constants.MomentsOfTheDayEnum.Sunrise;

      Debug.Log(
        $"[{nameof(LocationTourManager)}] Moment of the day changed: {this.currentMomentOfTheDayEntity.MomentOfTheDay}");

      this.MomentOfTheDayChanged?.Invoke(this);
    }

    protected override void UnloadApp(ExerciseEntity exerciseEntity, bool resetMode)
    {
      base.UnloadApp(exerciseEntity, resetMode);
    }

    private void SelectLocationEntity(LocationEntity locationEntity)
    {
      this.communicationManager?.SendMessage(new LocationEntity { Id = locationEntity.Id });

      // TODO: Esta logica del modo Sandbox no debe ir aca
      if (ApplicationHelper.ApplicationMode == ApplicationHelper.ApplicationModeEnum.Sandbox)
      {
        this.HandleLocationEntityReceived(locationEntity);
      }
    }

    private void HandleAppViewEntityReceived(AppViewEntity appViewEntity)
    {
      if (appViewEntity.AppView == Constants.AppViewEnum.Drone && this.CurrentMode != ModeEnum.DroneMode)
      {
        this.CurrentMode = ModeEnum.DroneMode;

        Debug.Log($"[{nameof(LocationTourManager)}] App mode changed: {this.CurrentMode}");

        this.ModeChanged?.Invoke(this);
      }
    }

    private void HandleLocationEntityReceived(LocationEntity locationEntity)
    {
      if (!object.Equals(this.currentLocationEntity.Id, locationEntity.Id)
          || this.CurrentMode != ModeEnum.PointOfInterestMode)
      {
        this.currentLocationEntity.Id = locationEntity.Id;
        this.currentLocationEntity.Node = locationEntity.Node;
        this.currentLocationEntity.Hint = locationEntity.Hint;
        this.currentLocationEntity.ScreenPoint.Vector2 = locationEntity.ScreenPoint.Vector2;

        this.CurrentMode = ModeEnum.PointOfInterestMode;

        this.ModeChanged?.Invoke(this);
      }
    }

    private void HandleLocationGuyEntityReceived(LocationGuyEntity locationGuyEntity)
    {
      // Force mode changed
      this.currentLocationGuyEntity.ScreenPoint.Vector2 = locationGuyEntity.ScreenPoint.Vector2;

      this.CurrentMode = ModeEnum.WalkMode;

      this.ModeChanged?.Invoke(this);
    }

    public struct WalkModeData
    {
      private readonly LocationTourManager locationTourManager;

      public WalkModeData(LocationTourManager locationTourManager)
      {
        this.locationTourManager = locationTourManager;
      }
    }

    public struct PointOfInteresetModeData
    {
      private readonly LocationTourManager locationTourManager;

      public PointOfInteresetModeData(LocationTourManager locationTourManager)
      {
        this.locationTourManager = locationTourManager;
      }

      public string Id => this.locationTourManager.currentLocationEntity.Id;

      public string Hint => this.locationTourManager.currentLocationEntity.Hint;

      public string Description => this.locationTourManager.currentLocationEntity.Description;
    }
  }
}