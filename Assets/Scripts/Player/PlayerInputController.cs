namespace Assets.Scripts.Player
{
  using Assets.Scripts.Input;

  using UnityEngine;
  using UnityEngine.InputSystem;

  [RequireComponent(typeof(PlayerController))]
  public class PlayerInputController : ApplicationInputBehaviour
  {
    private PlayerController playerController;

    protected override void Awake()
    {
      base.Awake();

      this.playerController = this.GetComponent<PlayerController>();
    }

    protected override void Subscribe(ApplicationInputActions inputActions)
    {
      inputActions.PlayerMovement.Movement.performed += this.HandleMovementChanged;
      inputActions.PlayerMovement.Movement.canceled += this.HandleMovementChanged;

      inputActions.PlayerMovement.Look.performed += this.HandleLookChanged;
      inputActions.PlayerMovement.Look.canceled += this.HandleLookChanged;

      inputActions.PlayerMovement.MaximumSpeed.performed += this.HandleMaximumSpeedChanged;
      inputActions.PlayerMovement.MaximumSpeed.canceled += this.HandleRegularSpeedChanged;
    }

    protected override void Unsubscribe(ApplicationInputActions inputActions)
    {
      inputActions.PlayerMovement.Movement.performed -= this.HandleMovementChanged;
      inputActions.PlayerMovement.Movement.canceled -= this.HandleMovementChanged;

      inputActions.PlayerMovement.Look.performed -= this.HandleLookChanged;
      inputActions.PlayerMovement.Look.canceled -= this.HandleLookChanged;

      inputActions.PlayerMovement.MaximumSpeed.performed -= this.HandleMaximumSpeedChanged;
      inputActions.PlayerMovement.MaximumSpeed.canceled -= this.HandleRegularSpeedChanged;
    }

    private void HandleMovementChanged(InputAction.CallbackContext callbackContext)
    {
      this.playerController.MovementInput = callbackContext.ReadValue<Vector2>();
    }

    private void HandleLookChanged(InputAction.CallbackContext callbackContext)
    {
      this.playerController.RotationInput = callbackContext.ReadValue<Vector2>();
    }

    private void HandleRegularSpeedChanged(InputAction.CallbackContext callbackContext)
    {
      this.playerController.RegularSpeed();
    }

    private void HandleMaximumSpeedChanged(InputAction.CallbackContext callbackContext)
    {
      this.playerController.MaximumSpeed();
    }
  }
}