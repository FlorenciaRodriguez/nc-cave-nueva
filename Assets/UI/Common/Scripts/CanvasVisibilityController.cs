#pragma warning disable 649
namespace Assets.UI.Common.Scripts
{
  using UnityEngine;

  using UnityEngine.InputSystem;

  [RequireComponent(typeof(Canvas))]
  public class CanvasVisibilityController : MonoBehaviour
  {
    [SerializeField]
    private bool initializeVisible = false;

    [SerializeField]
    private InputActionProperty inputAction;

    private Canvas canvas;

    public Canvas Canvas => this.canvas;

    protected void Awake()
    {
      this.canvas = this.GetComponent<Canvas>();
    }

    protected void Start()
    {
      if (this.inputAction.action != null)
      {
        this.inputAction.action.Enable();
        this.inputAction.action.performed += this.HandleToggleButton;
      }

      this.SetEnabled(this.initializeVisible);
    }

    protected void OnDestroy()
    {
      if (this.inputAction.action != null)
      {
        this.inputAction.action.performed -= this.HandleToggleButton;
      }
    }

    protected virtual void SetEnabled(bool value)
    {
      this.canvas.enabled = value;
      foreach (Transform child in this.canvas.transform)
      {
        child.gameObject.SetActive(value);
      }
    }

    protected virtual void HandleToggleButton(InputAction.CallbackContext value)
    {
      if (value.ReadValueAsButton())
      {
        this.SetEnabled(!this.canvas.enabled);
      }
    }
  }
}
