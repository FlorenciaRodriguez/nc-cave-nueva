#pragma warning disable 649
namespace Assets.TransitionAnimation.Scripts
{
  using System.Collections;

  using Cinemachine;

  using UnityEngine;

  public class UpDownTransitionAnimation : BaseTransitionAnimation
  {
    [SerializeField]
    private CinemachineBrain cinemachineBrain;

    [Space]

    [SerializeField]
    private CinemachineVirtualCamera downAuxiliaryInitialCamera;

    [SerializeField]
    private CinemachineVirtualCamera topAuxiliaryInitialCamera;

    [SerializeField]
    private CinemachineVirtualCamera downAuxiliaryTargetCamera;

    [SerializeField]
    private CinemachineVirtualCamera topAuxiliaryTargetCamera;

    [Space]

    [SerializeField]
    private float horizontalSpeed = 10.0f;

    [SerializeField]
    private float verticalSpeed = 5.0f;

    [SerializeField]
    private float height = 1f;

    [Space]

    [SerializeField]
    private int highPriority = 40;

    [SerializeField]
    private int lowPriority = 10;

    public override void StartAnimation(CameraTransform from, CameraTransform to)
    {
      this.StopAnimation();

      this.downAuxiliaryInitialCamera.transform.position = from.Position;
      this.downAuxiliaryInitialCamera.transform.rotation = from.Rotation;

      this.topAuxiliaryInitialCamera.transform.position = from.Position + new Vector3(0.0f, this.height, 0.0f);
      this.topAuxiliaryInitialCamera.transform.rotation = from.Rotation;

      this.downAuxiliaryTargetCamera.transform.position = to.Position;
      this.downAuxiliaryTargetCamera.transform.rotation = to.Rotation;

      this.topAuxiliaryTargetCamera.transform.position = to.Position + new Vector3(0.0f, this.height, 0.0f);
      this.topAuxiliaryTargetCamera.transform.rotation = to.Rotation;

      this.downAuxiliaryInitialCamera.Priority = this.highPriority;

      this.cinemachineBrain.m_CustomBlends.m_CustomBlends[1].m_Blend.m_Time =
        this.CalculateHorizontalTime(from.Position, to.Position);

      this.cinemachineBrain.m_CustomBlends.m_CustomBlends[2].m_Blend.m_Time = this.CalculateVerticalTime();
      this.cinemachineBrain.m_CustomBlends.m_CustomBlends[3].m_Blend.m_Time = this.CalculateVerticalTime();

      this.downAuxiliaryInitialCamera.enabled = true;
      this.topAuxiliaryInitialCamera.enabled = true;
      this.downAuxiliaryTargetCamera.enabled = true;
      this.topAuxiliaryTargetCamera.enabled = true;

      this.StartCoroutine(this.HandleAnimationCoroutine(from, to));
    }

    public override void StopAnimation()
    {
      if (this.IsExecuting)
      {
        this.StopAllCoroutines();

        this.InternalReset();
        this.OnTransitionStopped();
      }
    }

    protected void OnEnable()
    {
      this.InternalReset();
    }

    protected void OnDisable()
    {
      this.InternalReset();
    }

    protected void OnValidate()
    {
      this.height = Mathf.Max(1f, this.height);
      this.horizontalSpeed = Mathf.Max(0.1f, this.horizontalSpeed);
      this.verticalSpeed = Mathf.Max(0.1f, this.verticalSpeed);
      this.highPriority = Mathf.Max(this.lowPriority, this.highPriority);

      if (this.downAuxiliaryInitialCamera == null)
      {
        Debug.LogError(
          $"The variable '{nameof(this.downAuxiliaryInitialCamera)}' of  the component '{this.GetType().Name}' of the game object '{this.name}' has not been assigned.",
          this.gameObject);
      }

      if (this.topAuxiliaryInitialCamera == null)
      {
        Debug.LogError(
          $"The variable '{nameof(this.topAuxiliaryInitialCamera)}' of  the component '{this.GetType().Name}' of the game object '{this.name}' has not been assigned.",
          this.gameObject);
      }

      if (this.downAuxiliaryTargetCamera == null)
      {
        Debug.LogError(
          $"The variable '{nameof(this.downAuxiliaryTargetCamera)}' of  the component '{this.GetType().Name}' of the game object '{this.name}' has not been assigned.",
          this.gameObject);
      }

      if (this.topAuxiliaryTargetCamera == null)
      {
        Debug.LogError(
          $"The variable '{nameof(this.topAuxiliaryTargetCamera)}' of  the component '{this.GetType().Name}' of the game object '{this.name}' has not been assigned.",
          this.gameObject);
      }

      if (this.gameObject.scene.isLoaded)
      { 
        if (this.cinemachineBrain == null)
        {
          Debug.LogError(
            $"The variable '{nameof(this.cinemachineBrain)}' of  the component '{this.GetType().Name}' of the game object '{this.name}' has not been assigned.",
            this.gameObject);
        }
      }
    }

    private IEnumerator HandleAnimationCoroutine(CameraTransform from, CameraTransform to)
    {
      this.OnTransitionStarted(from, to);

      yield return null;

      this.topAuxiliaryInitialCamera.Priority = this.highPriority;
      this.downAuxiliaryInitialCamera.Priority = this.lowPriority;

      while (true)
      {
        if (!CinemachineCore.Instance.IsLive(this.downAuxiliaryInitialCamera) && CinemachineCore.Instance.IsLive(this.topAuxiliaryInitialCamera))
        {
          this.topAuxiliaryTargetCamera.Priority = this.highPriority;
          this.topAuxiliaryInitialCamera.Priority = this.lowPriority;
        }

        if (!CinemachineCore.Instance.IsLive(this.topAuxiliaryInitialCamera) && CinemachineCore.Instance.IsLive(this.topAuxiliaryTargetCamera))
        {
          this.downAuxiliaryTargetCamera.Priority = this.highPriority;
          this.topAuxiliaryTargetCamera.Priority = this.lowPriority;
        }

        if (!CinemachineCore.Instance.IsLive(this.topAuxiliaryTargetCamera) && CinemachineCore.Instance.IsLive(this.downAuxiliaryTargetCamera))
        {
          this.downAuxiliaryTargetCamera.Priority = this.lowPriority;

          this.InternalReset();
          this.OnTransitionFinished(from, to);

          yield break;
        }

        yield return null;
      }
    }

    private float CalculateHorizontalTime(Vector3 from, Vector3 to)
    {
      return Vector3.Distance(from, to) / this.horizontalSpeed;
    }

    private float CalculateVerticalTime()
    {
      return this.height / this.verticalSpeed;
    }

    private void InternalReset()
    {
      this.downAuxiliaryInitialCamera.enabled = false;
      this.topAuxiliaryInitialCamera.enabled = false;
      this.downAuxiliaryTargetCamera.enabled = false;
      this.topAuxiliaryTargetCamera.enabled = false;

      this.downAuxiliaryInitialCamera.Priority = this.lowPriority;
      this.topAuxiliaryInitialCamera.Priority = this.lowPriority;
      this.downAuxiliaryTargetCamera.Priority = this.lowPriority;
      this.topAuxiliaryTargetCamera.Priority = this.lowPriority;
    }
  }
}