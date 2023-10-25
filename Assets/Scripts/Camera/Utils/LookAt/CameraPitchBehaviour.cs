namespace Assets.Scripts.Camera.Utils.LookAt
{
  using System;
  using System.Collections;

  using Cinemachine;

  using UnityEngine;

  public class CameraPitchBehaviour : MonoBehaviour
  {
    [SerializeField]
    private CinemachineVirtualCamera cinemachineVirtualCamera = null;

    [Space]

    [SerializeField]
    private RotationSettings pitchSettings = new RotationSettings { LowerLimit = -30, UpperLimit = 30, SmoothTime = 1f };

    private float rotationInput = 0.0f;

    private LookingEnum looking = LookingEnum.Center;

    public enum LookingEnum
    {
      Center,

      Up,

      Down
    }

    public float RotationInput
    {
      get => this.rotationInput;
      set
      {
        this.rotationInput = value;

        if (this.rotationInput > 0.3f)
        {
          this.LookAt(CameraPitchBehaviour.LookingEnum.Up, false);
        }
        else if (this.rotationInput < -0.3f)
        {
          this.LookAt(CameraPitchBehaviour.LookingEnum.Down, false);
        }
        else
        {
          this.LookAt(CameraPitchBehaviour.LookingEnum.Center, false);
        }
      }
    }

    public RotationSettings PitchSettings => this.pitchSettings;

    public LookingEnum Looking => this.looking;

    public float CurrentPitch => Mathf.DeltaAngle(0, this.cinemachineVirtualCamera.transform.localEulerAngles.x);

    public void LookAt(LookingEnum targetLooking, bool force)
    {
      this.looking = targetLooking;
      var targetPitch = 0f;
      switch (targetLooking)
      {
        case LookingEnum.Up:
          targetPitch = this.pitchSettings.LowerLimit;
          break;
        case LookingEnum.Down:
          targetPitch = this.pitchSettings.UpperLimit;
          break;
      }

      if (this.pitchSettings.AnimationCoroutine != null)
      {
        this.StopCoroutine(this.pitchSettings.AnimationCoroutine);
        this.pitchSettings.AnimationCoroutine = null;
        this.pitchSettings.AnimationVelocity = 0;
      }

      if (force)
      {
        this.UpdatePitch(targetPitch);
      }
      else
      {
        this.pitchSettings.AnimationCoroutine = this.StartCoroutine(this.AnimatePitch(targetPitch));
      }
    }

    protected void OnValidate()
    {
      this.Validate(ref this.pitchSettings);

      if (this.cinemachineVirtualCamera == null)
      {
        Debug.LogError(
          $"The variable '{nameof(this.cinemachineVirtualCamera)}' of  the component '{this.GetType().Name}' of the game object '{this.name}' has not been assigned.",
          this.gameObject);
      }
    }

    private void UpdatePitch(float value)
    {
      this.cinemachineVirtualCamera.transform.localRotation = Quaternion.Euler(value, this.cinemachineVirtualCamera.transform.localEulerAngles.y, this.cinemachineVirtualCamera.transform.localEulerAngles.z);
    }

    private IEnumerator AnimatePitch(float targetPitch)
    {
      var currentPitch = this.CurrentPitch;
      var velocity = this.pitchSettings.AnimationVelocity;
      while (Mathf.Abs(Mathf.DeltaAngle(currentPitch, targetPitch)) > 0.01f)
      {
        this.UpdatePitch(Mathf.SmoothDampAngle(currentPitch, targetPitch, ref velocity, this.pitchSettings.SmoothTime));
        this.pitchSettings.AnimationVelocity = velocity;
        yield return null;
        currentPitch = this.CurrentPitch;
      }

      this.UpdatePitch(targetPitch);
      this.pitchSettings.AnimationCoroutine = null;
      this.pitchSettings.AnimationVelocity = 0;
    }

    private void Validate(ref RotationSettings rotationSettings)
    {
      rotationSettings.LowerLimit = Mathf.Min(rotationSettings.LowerLimit, 0);
      rotationSettings.UpperLimit = Mathf.Max(rotationSettings.LowerLimit, rotationSettings.UpperLimit);
      rotationSettings.SmoothTime = Mathf.Max(rotationSettings.SmoothTime, 0);
    }

    [Serializable]
    public struct RotationSettings
    {
      public float LowerLimit;

      public float UpperLimit;

      public float SmoothTime;

      [HideInInspector]
      public Coroutine AnimationCoroutine;

      [HideInInspector]
      public float AnimationVelocity;
    }
  }
}