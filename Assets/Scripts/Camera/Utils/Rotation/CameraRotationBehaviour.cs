#pragma warning disable 649
namespace Assets.Scripts.Camera.Utils.Rotation
{
  using Cinemachine;

  using UnityEngine;

  public class CameraRotationBehaviour : MonoBehaviour
  {
    [SerializeField]
    private CinemachineVirtualCamera cinemachineVirtualCamera;

    [Space]

    [SerializeField]
    private float rotationSensitivity = 60f;

    [Space]

    [SerializeField]
    private bool useHorizontalLimits;

    [SerializeField]
    private float horizontalLimit = 45f;

    [Space]

    [SerializeField]
    private bool useVerticalLimits;

    [SerializeField]
    private float verticalLimit = 45f;

    private Vector3 rotationNormalize = Vector3.zero;

    private float inputRotationY;

    private float inputRotationX;

    private float rotationX;

    private float rotationY;

    private Vector2 rotationInput = Vector2.zero;

    public Vector2 RotationInput
    {
      get => this.rotationInput;
      set
      {
        this.rotationInput = value;
        this.rotationNormalize = Vector3.Normalize(this.rotationInput);
      }
    }

    public CinemachineVirtualCamera CinemachineVirtualCamera => this.cinemachineVirtualCamera;

    public void Reset()
    {
      this.RotationInput = Vector2.zero;

      this.inputRotationX = 0f;
      this.inputRotationY = 0f;

      this.rotationX = 0f;
      this.rotationY = 0f;

      this.cinemachineVirtualCamera.transform.localRotation = Quaternion.identity;
    }

    protected void Awake()
    {
      this.Reset();
    }

    protected void OnEnable()
    {
      this.cinemachineVirtualCamera.enabled = true;
    }

    protected void OnDisable()
    {
      this.cinemachineVirtualCamera.enabled = false;
    }

    protected void Update()
    {
      this.inputRotationY = this.rotationNormalize.x * this.rotationSensitivity * Time.deltaTime;
      this.inputRotationX = this.rotationNormalize.y * this.rotationSensitivity * Time.deltaTime;

      this.rotationX -= this.inputRotationX;
      if (this.useVerticalLimits)
      {
        this.rotationX = Mathf.Clamp(this.rotationX, -this.verticalLimit, this.verticalLimit);
      }

      this.rotationY += this.inputRotationY;
      if (this.useHorizontalLimits)
      {
        this.rotationY = Mathf.Clamp(this.rotationY, -this.horizontalLimit, this.horizontalLimit);
      }

      this.cinemachineVirtualCamera.transform.localRotation = Quaternion.Euler(this.rotationX, this.rotationY, 0f);

      var eulerAngles = this.cinemachineVirtualCamera.transform.eulerAngles;
      eulerAngles.z = 0;
      this.cinemachineVirtualCamera.transform.eulerAngles = eulerAngles;
    }

    protected void OnValidate()
    {
      if (this.cinemachineVirtualCamera == null)
      {
        Debug.LogError(
          $"The variable '{nameof(this.cinemachineVirtualCamera)}' of  the component '{this.GetType().Name}' of the game object '{this.name}' has not been assigned.",
          this.gameObject);
      }
    }
  }
}