#pragma warning disable 649
namespace Assets.Scripts.Player
{
  using Cinemachine;

  using UnityEngine;

  [RequireComponent(typeof(CharacterController))]
  public class PlayerController : MonoBehaviour
  {
    [SerializeField]
    private CinemachineVirtualCamera cinemachineVirtualCamera;

    [Space]

    [SerializeField]
    private float regularSpeed = 3f;

    [SerializeField]
    private float maximumSpeed = 10f;

    [SerializeField]
    private float rotationSensitivity = 60f;

    [Space]

    [SerializeField]
    private LayerMask groundLayerMask = Physics.AllLayers;

    [SerializeField]
    private float raycastHeightOffset = 100f;

    private CharacterController characterController;

    private float speed;

    private Vector3 move = Vector3.zero;

    private float rotationY;

    private float inputRotationX;

    private Vector3 rotationNormalize = Vector3.zero;

    private Vector2 movementInput = Vector2.zero;

    private Vector2 rotationInput = Vector2.zero;

    public Vector2 MovementInput
    {
      get => this.movementInput;
      set => this.movementInput = value;
    }

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

    public void MaximumSpeed()
    {
      this.speed = this.maximumSpeed;
    }

    public void RegularSpeed()
    {
      this.speed = this.regularSpeed;
    }

    public void PlacePlayer(Vector3 targetPosition, Quaternion rotation)
    {
      RaycastHit hit;
      if (Physics.Raycast(targetPosition + new Vector3(0f, this.raycastHeightOffset, 0f), Vector3.down, out hit, float.MaxValue, this.groundLayerMask))
      {
        targetPosition = hit.point + new Vector3(0f, this.characterController.skinWidth);
      }

      this.transform.SetPositionAndRotation(targetPosition, rotation);
    }

    public void Reset()
    {
      this.transform.SetPositionAndRotation(Vector3.zero, Quaternion.identity);
      this.cinemachineVirtualCamera.transform.localRotation = Quaternion.identity;

      this.move = Vector3.zero;
      this.inputRotationX = 0f;

      this.MovementInput = Vector2.zero;
      this.RotationInput = Vector2.zero;
    }

    protected void Awake()
    {
      this.characterController = this.GetComponent<CharacterController>();
      this.speed = this.regularSpeed;
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
      this.move = (this.transform.right * this.movementInput.x * this.speed)
                  + (this.transform.forward * this.movementInput.y * this.speed) + Physics.gravity;
      this.characterController.Move(this.move * Time.deltaTime);

      this.inputRotationX = this.rotationNormalize.x * this.rotationSensitivity * Time.deltaTime;

      this.transform.Rotate(Vector3.up * this.inputRotationX);
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