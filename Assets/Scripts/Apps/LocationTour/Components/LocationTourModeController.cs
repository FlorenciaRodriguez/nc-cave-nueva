#pragma warning disable 649
namespace Assets.Scripts.Apps.LocationTour.Components
{
  using System;

  using Assets.Scripts.Adic.Utils;

  using MediaLab.Adic.Framework.Attributes;

  using UnityEngine;

  [RequireComponent(typeof(LocationTourTransitionModeController))]
  public class LocationTourModeController : BaseBehaviour
  {
    [Inject]
    private LocationTourManager locationTourManager;

    private LocationTourTransitionModeController locationTourTransitionModeController;

    protected void Awake()
    {
      this.locationTourTransitionModeController = this.GetComponent<LocationTourTransitionModeController>();
    }

    protected override void Start()
    {
      base.Start();

      if (this.locationTourManager != null)
      {
        this.locationTourManager.ModeChanged += this.HandleModeChanged;
        this.ActiveMode(this.locationTourManager.CurrentMode);
      }
      else
      {
        throw new Exception($"The reference '{nameof(LocationTourManager)}' of  the component '{this.GetType().Name}' of the game object '{this.name}' has not been assigned");
      }
    }

    protected void OnDestroy()
    {
      if (this.locationTourManager != null)
      {
        this.locationTourManager.ModeChanged -= this.HandleModeChanged;
        this.locationTourManager = null;
      }
    }

    private void HandleModeChanged(LocationTourManager manager)
    {
      this.ActiveMode(this.locationTourManager.CurrentMode);
    }

    private void ActiveMode(LocationTourManager.ModeEnum currentMode)
    {
      switch (currentMode)
      {
        case LocationTourManager.ModeEnum.None:
          this.ActiveNoneMode();
          break;
          
        case LocationTourManager.ModeEnum.WalkMode:
          this.ActiveWalkMode(this.locationTourManager.WalkData);
          break;

        case LocationTourManager.ModeEnum.DroneMode:
          this.ActiveDroneMode();
          break;
      }
    }

    private void ActiveNoneMode()
    {
      this.locationTourTransitionModeController.ActiveNoneMode();
    }
   
    private void ActiveWalkMode(LocationTourManager.WalkModeData data)
    {
      this.locationTourTransitionModeController.TransitionToWalkMode();
    }

    private void ActiveDroneMode()
    {
      this.locationTourTransitionModeController.TransitionToDroneMode();
    }
  }
}
