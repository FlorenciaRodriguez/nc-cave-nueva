#pragma warning disable 649
namespace Assets.UI.Common.Scripts
{
  using UnityEngine;
  using UnityEngine.InputSystem;
  using UnityEngine.UI;

  [RequireComponent(typeof(Button))]
  public class ButtonShortcut : MonoBehaviour
  {
    [SerializeField]
    private InputActionProperty inputActionProperty;

    private Button button;

    protected void Awake()
    {
      this.button = this.GetComponent<Button>();
    }

    protected void OnEnable()
    {
      if (this.inputActionProperty.action != null)
      {
        this.inputActionProperty.action.Enable();
        this.inputActionProperty.action.performed += this.HandleInputActionPerformed;
      }
    }

    protected void OnDisable()
    {
      if (this.inputActionProperty.action != null)
      {
        this.inputActionProperty.action.performed -= this.HandleInputActionPerformed;
      }
    }

    private void HandleInputActionPerformed(InputAction.CallbackContext callbackContext)
    {
      if (this.button.enabled && this.button.interactable)
      {
        var rootCanvas = this.GetComponentInParent<Canvas>(true).rootCanvas;
        if (rootCanvas != null && rootCanvas.isActiveAndEnabled)
        {
          this.button.onClick.Invoke();
        }
      }
    }
  }
}
