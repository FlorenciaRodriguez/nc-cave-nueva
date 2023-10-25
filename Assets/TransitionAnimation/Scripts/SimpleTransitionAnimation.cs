#pragma warning disable 649
namespace Assets.TransitionAnimation.Scripts
{
  using System.Collections;

  using Cinemachine;

  using UnityEngine;

  public class SimpleTransitionAnimation : BaseTransitionAnimation
  {
    [SerializeField]
    private CinemachineBrain cinemachineBrain;

    [Space]

    [SerializeField]
    private CinemachineVirtualCamera auxiliaryInitialCamera;

    [SerializeField]
    private CinemachineVirtualCamera auxiliaryTargetCamera;

    [Space]

    [SerializeField]
    private float speed = 10.0f;

    [Space]

    [SerializeField]
    private int highPriority = 40;

    [SerializeField]
    private int lowPriority = 10;

    public override void StartAnimation(CameraTransform from, CameraTransform to)
    {
      this.StopAnimation();

      this.auxiliaryInitialCamera.transform.position = from.Position;
      this.auxiliaryInitialCamera.transform.rotation = from.Rotation;

      this.auxiliaryTargetCamera.transform.position = to.Position;
      this.auxiliaryTargetCamera.transform.rotation = to.Rotation;

      this.auxiliaryInitialCamera.Priority = this.highPriority;
      this.auxiliaryTargetCamera.Priority = this.lowPriority;

      this.cinemachineBrain.m_CustomBlends.m_CustomBlends[0].m_Blend.m_Time =
        this.CalculateTime(from.Position, to.Position);

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
      this.StopAnimation();
    }

    protected void OnValidate()
    {
      this.speed = Mathf.Max(0.1f, this.speed);
      this.highPriority = Mathf.Max(this.lowPriority, this.highPriority);

      if (this.auxiliaryInitialCamera == null)
      {
        Debug.LogError(
          $"The variable '{nameof(this.auxiliaryInitialCamera)}' of  the component '{this.GetType().Name}' of the game object '{this.name}' has not been assigned.",
          this.gameObject);
      }

      if (this.auxiliaryTargetCamera == null)
      {
        Debug.LogError(
          $"The variable '{nameof(this.auxiliaryTargetCamera)}' of  the component '{this.GetType().Name}' of the game object '{this.name}' has not been assigned.",
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

      this.auxiliaryInitialCamera.enabled = true;
      this.auxiliaryTargetCamera.enabled = true;

      this.auxiliaryInitialCamera.Priority = this.highPriority;
      this.auxiliaryTargetCamera.Priority = this.lowPriority;

      yield return null;

      this.auxiliaryInitialCamera.Priority = this.lowPriority;
      this.auxiliaryTargetCamera.Priority = this.highPriority;

      while (true)
      {
        if (!CinemachineCore.Instance.IsLive(this.auxiliaryInitialCamera) && CinemachineCore.Instance.IsLive(this.auxiliaryTargetCamera))
        {
          this.auxiliaryTargetCamera.Priority = this.lowPriority;
          
          this.InternalReset();
          this.OnTransitionFinished(from, to);

          yield break;
        }

        yield return null;
      }
    }

    private float CalculateTime(Vector3 from, Vector3 to)
    {
      return Vector3.Distance(from, to) / this.speed;
    }

    private void InternalReset()
    {
      this.auxiliaryInitialCamera.enabled = false;
      this.auxiliaryTargetCamera.enabled = false;

      this.auxiliaryInitialCamera.Priority = this.lowPriority;
      this.auxiliaryTargetCamera.Priority = this.lowPriority;
    }
  }
}