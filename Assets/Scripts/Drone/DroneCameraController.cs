#pragma warning disable 649
namespace Assets.Scripts.Drone
{
  using Cinemachine;

  using UnityEngine;

  [RequireComponent(typeof(CharacterController))]
  public class DroneCameraController : MonoBehaviour
  {
    [SerializeField]
    private CinemachineVirtualCamera cinemachineVirtualCamera;

    [Space]

    [SerializeField]
    private float horizontalSpeed = 3f;

    [SerializeField]
    private float maximumHorizontalSpeed = 8f;

    [SerializeField]
    private float verticalSpeed = 2f;

    [SerializeField]
    private float rotationSensitivity = 100f;

    private float speed;

    private CharacterController characterController;

    private float rotationX;

    private Vector2 rotationInput = Vector2.zero;

    private Vector2 moveHorizontalInput = Vector2.zero;

    private float moveVerticalInput;

    public Vector2 MoveHorizontalInput
    {
      get => this.moveHorizontalInput;
      set => this.moveHorizontalInput = value;
    }

    public float MoveVerticalInput
    {
      get => this.moveVerticalInput;
      set => this.moveVerticalInput = value;
    }

    public Vector2 RotationInput
    {
      get => this.rotationInput;
      set => this.rotationInput = Vector3.Normalize(value);
    }

    public CinemachineVirtualCamera CinemachineVirtualCamera => this.cinemachineVirtualCamera;

    public void MaximumSpeed()
    {
      this.speed = this.maximumHorizontalSpeed;
    }

    public void RegularSpeed()
    {
      this.speed = this.horizontalSpeed;
    }

    public void Reset()
    {
      this.transform.SetPositionAndRotation(Vector3.zero, Quaternion.identity);
      this.cinemachineVirtualCamera.transform.localRotation = Quaternion.identity;

      this.rotationX = 0f;
      this.MoveHorizontalInput = Vector2.zero;

      this.MoveVerticalInput = 0f;
      this.RotationInput = Vector2.zero;
    }

    protected void Awake()
    {
      this.characterController = this.GetComponent<CharacterController>();
      this.speed = this.horizontalSpeed;
    }

    protected void OnEnable()
    {
      this.characterController.enabled = true;
      this.cinemachineVirtualCamera.enabled = true;
    }

    protected void OnDisable()
    {
      this.characterController.enabled = false;
      this.cinemachineVirtualCamera.enabled = false;
    }

    protected void Update()
    {
      var move = (this.transform.right * this.moveHorizontalInput.x * this.speed)
                  + (this.transform.up * this.moveVerticalInput * this.verticalSpeed)
                  + (this.transform.forward * this.moveHorizontalInput.y * this.speed);

      this.characterController.Move(move * Time.deltaTime);

      this.transform.Rotate(Vector3.up * this.rotationInput.x * this.rotationSensitivity * Time.deltaTime);
      this.rotationX -= this.rotationInput.y * this.rotationSensitivity * Time.deltaTime;
      this.rotationX = Mathf.Clamp(this.rotationX, -90f, 90f);
      this.cinemachineVirtualCamera.transform.localRotation = Quaternion.Euler(this.rotationX, 0f, 0f);
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