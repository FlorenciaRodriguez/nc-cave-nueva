namespace Assets.Scripts.Camera.Utils.LookAt
{
  using Assets.Scripts.Input;

  using UnityEngine;
  using UnityEngine.InputSystem;

  [RequireComponent(typeof(CameraPitchBehaviour))]
  public class CameraPitchInputController : ApplicationInputBehaviour
  {
    private CameraPitchBehaviour cameraPitchBehaviour = null;

    protected override void Awake()
    {
      base.Awake();

      this.cameraPitchBehaviour = this.GetComponent<CameraPitchBehaviour>();
    }

    protected override void Subscribe(ApplicationInputActions inputActions)
    {
      inputActions.PlayerMovement.LookInX.performed += this.HandleLookChanged;
      inputActions.PlayerMovement.LookInX.canceled += this.HandleLookChanged;
    }

    protected override void Unsubscribe(ApplicationInputActions inputActions)
    {
      inputActions.PlayerMovement.LookInX.performed -= this.HandleLookChanged;
      inputActions.PlayerMovement.LookInX.canceled -= this.HandleLookChanged;
    }

    private void HandleLookChanged(InputAction.CallbackContext callbackContext)
    {
      this.cameraPitchBehaviour.RotationInput = callbackContext.ReadValue<float>();
    }
  }
}