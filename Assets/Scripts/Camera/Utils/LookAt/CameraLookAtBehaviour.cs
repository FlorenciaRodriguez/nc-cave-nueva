#pragma warning disable 649
namespace Assets.Scripts.Camera.Utils.LookAt
{
  using System.Collections;

  using Cinemachine;

  using UnityEngine;
  using UnityEngine.Events;

  public class CameraLookAtBehaviour : MonoBehaviour
  {
    [SerializeField]
    private CinemachineVirtualCamera cinemachineVirtualCamera;

    [SerializeField]
    private float rotationSpeed = 1f;

    public event UnityAction<CameraLookAtBehaviour, Vector3, Quaternion> Started; 

    public event UnityAction<CameraLookAtBehaviour, Vector3, Quaternion> Finished; 

    public event UnityAction<CameraLookAtBehaviour> Stopped;

    public bool IsExecuting { get; private set; }

    public Vector3 CurrentTarget { get; private set; }

    public CinemachineVirtualCamera CinemachineVirtualCamera => this.cinemachineVirtualCamera;

    public void StartLookAt(Vector3 target)
    {
      this.StopLookAt();

      this.StartCoroutine(this.HandleLookAt(target));
    }

    public void StopLookAt()
    {
      if (this.IsExecuting)
      {
        this.StopAllCoroutines();
        this.IsExecuting = false;
        this.CurrentTarget = Vector3.zero;

        this.Stopped?.Invoke(this);
      }
    }

    protected void OnEnable()
    {
      this.cinemachineVirtualCamera.enabled = true;
    }

    protected void OnDisable()
    {
      this.cinemachineVirtualCamera.enabled = false;
    }

    protected void OnValidate()
    {
      this.rotationSpeed = Mathf.Max(1f, this.rotationSpeed);

      if (this.cinemachineVirtualCamera == null)
      {
        Debug.LogError(
          $"The variable '{nameof(this.cinemachineVirtualCamera)}' of  the component '{this.GetType().Name}' of the game object '{this.name}' has not been assigned.",
          this.gameObject);
      }
    }

    private IEnumerator HandleLookAt(Vector3 target)
    {
      this.IsExecuting = true;
      this.CurrentTarget = target;

      var targetRotation = Quaternion.LookRotation(target - this.cinemachineVirtualCamera.transform.position);
      var initialRotation = this.cinemachineVirtualCamera.transform.rotation;

      var deltaAngleY = Mathf.DeltaAngle(initialRotation.eulerAngles.y, targetRotation.eulerAngles.y);
      var timeY = Mathf.Abs(deltaAngleY / this.rotationSpeed);

      var deltaAngleX = Mathf.DeltaAngle(initialRotation.eulerAngles.x, targetRotation.eulerAngles.x);
      var timeX = Mathf.Abs(deltaAngleX / this.rotationSpeed);

      var time = timeY > timeX ? timeY : timeX;

      this.Started?.Invoke(this, target, initialRotation);

      var currentTime = 0f;
      while (currentTime <= time)
      {
        this.cinemachineVirtualCamera.transform.rotation = Quaternion.Slerp(initialRotation, targetRotation, Mathf.InverseLerp(0, time, currentTime));
        currentTime += Time.deltaTime;

        yield return null;
      }

      this.cinemachineVirtualCamera.transform.rotation = targetRotation;

      this.IsExecuting = false;
      this.CurrentTarget = Vector3.zero;

      this.Finished?.Invoke(this, target, targetRotation);
    }
  }
}