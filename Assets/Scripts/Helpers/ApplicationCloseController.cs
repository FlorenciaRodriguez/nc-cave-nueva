#pragma warning disable 649
namespace Assets.Scripts.Helpers
{
  using UnityEngine;
  using UnityEngine.InputSystem;

  public class ApplicationCloseController : MonoBehaviour
  {
    [SerializeField]
    private InputActionProperty inputAction;

    protected void Start()
    {
      if (this.inputAction.action != null)
      {
        this.inputAction.action.Enable();
        this.inputAction.action.performed += this.HandleCloseActionPerformed;
      }
    }

    protected void OnDestroy()
    {
      if (this.inputAction.action != null)
      {
        this.inputAction.action.performed -= this.HandleCloseActionPerformed;
      }
    }

    private void HandleCloseActionPerformed(InputAction.CallbackContext context)
    {
      if (context.ReadValueAsButton())
      {
        ApplicationHelper.Exit();
      }
    }
  }
}
