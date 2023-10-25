#pragma warning disable 649
namespace Assets.Scripts.Apps.LocationTour.Components
{
  using System;
  using System.Collections.Generic;
  using System.Diagnostics.CodeAnalysis;

  using Assets.Scripts.Camera.Utils.Rotation;
  using Assets.Scripts.Drone;
  using Assets.Scripts.Player;
  using Assets.TransitionAnimation.Scripts;

  using Cinemachine;

  using UnityEngine;
  using UnityEngine.Events;

  [SuppressMessage("ReSharper", "CollectionNeverUpdated.Local", Justification = "Reviewed Ok")]
  public class LocationTourTransitionModeController : MonoBehaviour
  {
    public const string Tag = "TransitionController";

    [SerializeField]
    private CinemachineBrain cinemachineBrain;

    [Space]

    [SerializeField]
    private CinemachineVirtualCamera homeVirtualCamera;

    [SerializeField]
    private BaseTransitionAnimation defaultTransitionAnimation;

    [Space]

    [SerializeField]
    private PlayerController playerController;

    [SerializeField]
    private List<TransitionModeAnimation> transitionToWalkModeAnimations = new List<TransitionModeAnimation>();

    [Space]

    [SerializeField]
    private DroneCameraController droneCameraController;

    [SerializeField]
    private List<TransitionModeAnimation> transitionToDroneModeAnimations = new List<TransitionModeAnimation>();

    [SerializeField]
    private float heightOffset = 5f;

    [Space]

    [SerializeField]
    private CameraRotationBehaviour cameraRotationBehaviour;

    [SerializeField]
    private List<TransitionModeAnimation> transitionToPointOfInterestModeAnimations = new List<TransitionModeAnimation>();

    [Space]

    [SerializeField]
    private int highPriority = 30;

    [SerializeField]
    private int lowPriority = 10;

    private BaseTransitionAnimation activeTransitionAnimation;

    public event UnityAction<LocationTourManager.ModeEnum> TransitionStarted;

    public event UnityAction<LocationTourManager.ModeEnum> TransitionFinished;

    public event UnityAction<LocationTourManager.ModeEnum> TransitionStopped;

    public LocationTourManager.ModeEnum CurrentMode { get; private set; } = LocationTourManager.ModeEnum.None;

    public bool IsExecuting { get; private set; }

    public void ActiveNoneMode()
    {
      this.activeTransitionAnimation?.StopAnimation();
      this.activeTransitionAnimation = null;

      this.OnStartTransition(LocationTourManager.ModeEnum.None);

      this.homeVirtualCamera.Priority = this.highPriority;
      this.DisableModeControllers();

      this.OnFinishTransition();
    }

    public void TransitionToWalkMode()
    {
      var from = new BaseTransitionAnimation.CameraTransform
      {
        Position = this.cinemachineBrain.transform.position,
        Rotation = this.cinemachineBrain.transform.rotation
      };

      this.homeVirtualCamera.Priority = this.lowPriority;
      this.DisableModeControllers();
  
      this.playerController.PlacePlayer(this.cinemachineBrain.transform.position, Quaternion.identity.normalized);

      var to = new BaseTransitionAnimation.CameraTransform
      {
        Position = this.playerController.CinemachineVirtualCamera.transform.position,
        Rotation = this.playerController.CinemachineVirtualCamera.transform.rotation
      };

      this.activeTransitionAnimation?.StopAnimation();
      this.activeTransitionAnimation = this.FindTransitionAnimation(this.transitionToWalkModeAnimations);

      this.OnStartTransition(LocationTourManager.ModeEnum.WalkMode);

      this.activeTransitionAnimation.TransitionStopped -= this.HandleTransitionToWalkModeStopped;
      this.activeTransitionAnimation.TransitionStopped += this.HandleTransitionToWalkModeStopped;
      this.activeTransitionAnimation.TransitionFinished -= this.HandleTransitionToWalkModeCompleted;
      this.activeTransitionAnimation.TransitionFinished += this.HandleTransitionToWalkModeCompleted;
      this.activeTransitionAnimation.StartAnimation(from, to);
    }

    public void TransitionToDroneMode()
    {
      if (this.CurrentMode == LocationTourManager.ModeEnum.DroneMode)
      {
        return;
      }

      var from = new BaseTransitionAnimation.CameraTransform
      {
        Position = this.cinemachineBrain.transform.position,
        Rotation = this.cinemachineBrain.transform.rotation
      };

      this.homeVirtualCamera.Priority = this.lowPriority;
      this.DisableModeControllers();

      this.droneCameraController.transform.SetPositionAndRotation(
        this.cinemachineBrain.transform.position + new Vector3(0f, this.heightOffset, 0f),
        Quaternion.Euler(0f, this.cinemachineBrain.transform.eulerAngles.y, 0f));

      var to = new BaseTransitionAnimation.CameraTransform
      {
        Position = this.droneCameraController.CinemachineVirtualCamera.transform.position,
        Rotation = this.droneCameraController.CinemachineVirtualCamera.transform.rotation
      };

      this.activeTransitionAnimation?.StopAnimation();
      this.activeTransitionAnimation = this.FindTransitionAnimation(this.transitionToDroneModeAnimations);

      this.OnStartTransition(LocationTourManager.ModeEnum.DroneMode);

      this.activeTransitionAnimation.TransitionStopped -= this.HandleTransitionToDroneModeStopped;
      this.activeTransitionAnimation.TransitionStopped += this.HandleTransitionToDroneModeStopped;
      this.activeTransitionAnimation.TransitionFinished -= this.HandleTransitionToDroneModeCompleted;
      this.activeTransitionAnimation.TransitionFinished += this.HandleTransitionToDroneModeCompleted;
      this.activeTransitionAnimation.StartAnimation(from, to);
    }

    protected void OnValidate()
    {
      this.highPriority = Mathf.Max(this.lowPriority, this.highPriority);
      this.heightOffset = Mathf.Max(0f, this.heightOffset);

      if (this.cinemachineBrain == null)
      {
        Debug.LogError(
          $"The variable '{nameof(this.cinemachineBrain)}' of  the component '{this.GetType().Name}' of the game object '{this.name}' has not been assigned.",
          this.gameObject);
      }

      if (this.homeVirtualCamera == null)
      {
        Debug.LogError(
          $"The variable '{nameof(this.homeVirtualCamera)}' of  the component '{this.GetType().Name}' of the game object '{this.name}' has not been assigned.",
          this.gameObject);
      }

      if (this.defaultTransitionAnimation == null)
      {
        Debug.LogError(
          $"The variable '{nameof(this.defaultTransitionAnimation)}' of  the component '{this.GetType().Name}' of the game object '{this.name}' has not been assigned.",
          this.gameObject);
      }

      if (this.playerController == null)
      {
        Debug.LogError(
          $"The variable '{nameof(this.playerController)}' of  the component '{this.GetType().Name}' of the game object '{this.name}' has not been assigned.",
          this.gameObject);
      }

      for (var i = 0; i < this.transitionToWalkModeAnimations.Count; i++)
      {
        if (this.transitionToWalkModeAnimations[i].TransitionAnimation == null)
        {
          Debug.LogError(
            $"The variable '{nameof(TransitionModeAnimation.TransitionAnimation)}' of the element '{i}' of the list '{nameof(this.transitionToWalkModeAnimations)}' of  the component '{this.GetType().Name}' of the game object '{this.name}' has not been assigned.",
            this.gameObject);
        }
      }

      if (this.droneCameraController == null)
      {
        Debug.LogError(
          $"The variable '{nameof(this.droneCameraController)}' of  the component '{this.GetType().Name}' of the game object '{this.name}' has not been assigned.",
          this.gameObject);
      }

      for (var i = 0; i < this.transitionToDroneModeAnimations.Count; i++)
      {
        if (this.transitionToDroneModeAnimations[i].TransitionAnimation == null)
        {
          Debug.LogError(
            $"The variable '{nameof(TransitionModeAnimation.TransitionAnimation)}' of the element '{i}' of the list '{nameof(this.transitionToDroneModeAnimations)}' of  the component '{this.GetType().Name}' of the game object '{this.name}' has not been assigned.",
            this.gameObject);
        }
      }

      if (this.cameraRotationBehaviour == null)
      {
        Debug.LogError(
          $"The variable '{nameof(this.cameraRotationBehaviour)}' of  the component '{this.GetType().Name}' of the game object '{this.name}' has not been assigned.",
          this.gameObject);
      }

      for (var i = 0; i < this.transitionToPointOfInterestModeAnimations.Count; i++)
      {
        if (this.transitionToPointOfInterestModeAnimations[i].TransitionAnimation == null)
        {
          Debug.LogError(
            $"The variable '{nameof(TransitionModeAnimation.TransitionAnimation)}' of the element '{i}' of the list '{nameof(this.transitionToPointOfInterestModeAnimations)}' of  the component '{this.GetType().Name}' of the game object '{this.name}' has not been assigned.",
            this.gameObject);
        }
      }
    }

    private BaseTransitionAnimation FindTransitionAnimation(List<TransitionModeAnimation> transitionModeAnimations)
    {
      foreach (var transitionModeAnimation in transitionModeAnimations)
      {
        if (transitionModeAnimation.FromMode == this.CurrentMode)
        {
          return transitionModeAnimation.TransitionAnimation;
        }
      }

      return this.defaultTransitionAnimation;
    }

    private void DisableModeControllers()
    {
      this.playerController.Reset();
      this.playerController.enabled = false;
      this.playerController.CinemachineVirtualCamera.Priority = this.lowPriority;

      this.droneCameraController.Reset();
      this.droneCameraController.enabled = false;
      this.playerController.CinemachineVirtualCamera.Priority = this.lowPriority;

      this.cameraRotationBehaviour.Reset();
      this.cameraRotationBehaviour.enabled = false;
      this.cameraRotationBehaviour.CinemachineVirtualCamera.Priority = this.lowPriority;
    }

    private void OnStartTransition(LocationTourManager.ModeEnum mode)
    {
      this.CurrentMode = mode;
      this.IsExecuting = true;

      Debug.Log($"Transition to '{mode}' mode started", this.gameObject);
      this.TransitionStarted?.Invoke(this.CurrentMode);
    }

    private void OnFinishTransition()
    {
      if (this.IsExecuting)
      {
        this.IsExecuting = false;

        Debug.Log($"Transition to '{this.CurrentMode}' mode finished", this.gameObject);
        this.TransitionFinished?.Invoke(this.CurrentMode);
      }
    }

    private void OnStopTransition()
    {
      if (this.IsExecuting)
      {
        this.IsExecuting = false;

        Debug.Log($"Transition to '{this.CurrentMode}' mode stopped", this.gameObject);
        this.TransitionStopped?.Invoke(this.CurrentMode);
      }
    }

    private void HandleTransitionToWalkModeCompleted(
      BaseTransitionAnimation sender,
      BaseTransitionAnimation.CameraTransform from,
      BaseTransitionAnimation.CameraTransform to)
    {
      sender.TransitionStopped -= this.HandleTransitionToWalkModeStopped;
      sender.TransitionFinished -= this.HandleTransitionToWalkModeCompleted;

      this.playerController.enabled = true;
      this.playerController.CinemachineVirtualCamera.Priority = this.highPriority;

      this.activeTransitionAnimation = null;
      this.OnFinishTransition();
    }

    private void HandleTransitionToWalkModeStopped(BaseTransitionAnimation sender)
    {
      sender.TransitionStopped -= this.HandleTransitionToWalkModeStopped;
      sender.TransitionFinished -= this.HandleTransitionToWalkModeCompleted;

      this.activeTransitionAnimation = null;
      this.OnStopTransition();
    }

    private void HandleTransitionToDroneModeCompleted(
      BaseTransitionAnimation sender,
      BaseTransitionAnimation.CameraTransform from,
      BaseTransitionAnimation.CameraTransform to)
    {
      sender.TransitionStopped -= this.HandleTransitionToDroneModeStopped;
      sender.TransitionFinished -= this.HandleTransitionToDroneModeCompleted;

      this.droneCameraController.enabled = true;
      this.droneCameraController.CinemachineVirtualCamera.Priority = this.highPriority;

      this.activeTransitionAnimation = null;
      this.OnFinishTransition();
    }

    private void HandleTransitionToDroneModeStopped(BaseTransitionAnimation sender)
    {
      sender.TransitionStopped -= this.HandleTransitionToDroneModeStopped;
      sender.TransitionFinished -= this.HandleTransitionToDroneModeCompleted;

      this.activeTransitionAnimation = null;
      this.OnStopTransition();
    }

    private void HandleTransitionToPointOfInterestModeCompleted(
      BaseTransitionAnimation sender,
      BaseTransitionAnimation.CameraTransform from,
      BaseTransitionAnimation.CameraTransform to)
    {
      sender.TransitionStopped -= this.HandleTransitionToPointOfInterestModeStopped;
      sender.TransitionFinished -= this.HandleTransitionToPointOfInterestModeCompleted;

      this.cameraRotationBehaviour.Reset();
      this.cameraRotationBehaviour.enabled = true;
      this.cameraRotationBehaviour.CinemachineVirtualCamera.Priority = this.highPriority;

      this.activeTransitionAnimation = null;
      this.OnFinishTransition();
    }

    private void HandleTransitionToPointOfInterestModeStopped(BaseTransitionAnimation sender)
    {
      sender.TransitionStopped -= this.HandleTransitionToPointOfInterestModeStopped;
      sender.TransitionFinished -= this.HandleTransitionToPointOfInterestModeCompleted;

      this.activeTransitionAnimation = null;
      this.OnStopTransition();
    }

    [Serializable]
    public struct TransitionModeAnimation
    {
      public LocationTourManager.ModeEnum FromMode;

      public BaseTransitionAnimation TransitionAnimation;
    }
  }
}
