namespace Assets.Scripts.Camera.Utils.Rotation
{
  using Assets.Scripts.Input;

  using UnityEngine;
  using UnityEngine.InputSystem;

  [RequireComponent(typeof(CameraRotationBehaviour))]
  public class CameraRotationInputController : ApplicationInputBehaviour
  {
    private CameraRotationBehaviour cameraRotationBehaviour;

    protected override void Awake()
    {
      base.Awake();

      this.cameraRotationBehaviour = this.GetComponent<CameraRotationBehaviour>();
    }

    protected override void Subscribe(ApplicationInputActions inputActions)
    {
      inputActions.PlayerMovement.Look.performed += this.HandleLookChanged;
      inputActions.PlayerMovement.Look.canceled += this.HandleLookChanged;
    }

    protected override void Unsubscribe(ApplicationInputActions inputActions)
    {
      inputActions.PlayerMovement.Look.performed -= this.HandleLookChanged;
      inputActions.PlayerMovement.Look.canceled -= this.HandleLookChanged;
    }

    private void HandleLookChanged(InputAction.CallbackContext callbackContext)
    {
      this.cameraRotationBehaviour.RotationInput = callbackContext.ReadValue<Vector2>();
    }
  }
}