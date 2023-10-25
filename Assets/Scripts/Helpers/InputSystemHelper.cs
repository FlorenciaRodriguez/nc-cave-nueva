namespace Assets.Scripts.Helpers
{
  using UnityEngine.InputSystem;

  public static class InputSystemHelper
  {
    public static T FindInputDevice<T>() where T : InputDevice
    {
      foreach (var inputDevice in InputSystem.devices)
      {
        if (inputDevice is T)
        {
          return inputDevice as T;
        }
      }

      return null;
    }
  }
}
