namespace Assets.Scripts.Drone
{
  using Assets.Scripts.Input;

  using UnityEngine;
  using UnityEngine.InputSystem;

  [RequireComponent(typeof(DroneCameraController))]
  public class DroneCameraInputController : ApplicationInputBehaviour
  {
    private DroneCameraController droneCameraController;

    protected override void Awake()
    {
      base.Awake();

      this.droneCameraController = this.GetComponent<DroneCameraController>();
    }

    protected override void Subscribe(ApplicationInputActions inputActions)
    {
      inputActions.DroneMovement.DroneMovement.performed += this.HandleDroneMovementChanged;
      inputActions.DroneMovement.DroneMovement.canceled += this.HandleDroneMovementChanged;

      inputActions.DroneMovement.DroneLook.performed += this.HandleLookDroneChanged;
      inputActions.DroneMovement.DroneLook.canceled += this.HandleLookDroneChanged;

      inputActions.DroneMovement.DroneUpDown.performed += this.HandleDroneUpDownChanged;
      inputActions.DroneMovement.DroneUpDown.canceled += this.HandleDroneUpDownChanged;

      inputActions.DroneMovement.MaximumSpeed.performed += this.HandleMaximumSpeedChanged;
      inputActions.DroneMovement.MaximumSpeed.canceled += this.HandleRegularSpeedChanged;
    }

    protected override void Unsubscribe(ApplicationInputActions inputActions)
    {
      inputActions.DroneMovement.DroneMovement.performed -= this.HandleDroneMovementChanged;
      inputActions.DroneMovement.DroneMovement.canceled -= this.HandleDroneMovementChanged;

      inputActions.DroneMovement.DroneLook.performed -= this.HandleLookDroneChanged;
      inputActions.DroneMovement.DroneLook.canceled -= this.HandleLookDroneChanged;

      inputActions.DroneMovement.DroneUpDown.performed -= this.HandleDroneUpDownChanged;
      inputActions.DroneMovement.DroneUpDown.canceled -= this.HandleDroneUpDownChanged;

      inputActions.DroneMovement.MaximumSpeed.performed -= this.HandleMaximumSpeedChanged;
      inputActions.DroneMovement.MaximumSpeed.canceled -= this.HandleRegularSpeedChanged;
    }

    private void HandleDroneMovementChanged(InputAction.CallbackContext callbackContext)
    {
      this.droneCameraController.MoveHorizontalInput = callbackContext.ReadValue<Vector2>();
    }

    private void HandleLookDroneChanged(InputAction.CallbackContext callbackContext)
    {
      this.droneCameraController.RotationInput = callbackContext.ReadValue<Vector2>();
    }

    private void HandleDroneUpDownChanged(InputAction.CallbackContext callbackContext)
    {
      this.droneCameraController.MoveVerticalInput = callbackContext.ReadValue<float>();
    }

    private void HandleRegularSpeedChanged(InputAction.CallbackContext callbackContext)
    {
      this.droneCameraController.RegularSpeed();
    }

    private void HandleMaximumSpeedChanged(InputAction.CallbackContext callbackContext)
    {
      this.droneCameraController.MaximumSpeed();
    }
  }
}