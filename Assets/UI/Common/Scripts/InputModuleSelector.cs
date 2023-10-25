namespace Assets.UI.Common.Scripts
{
  using UnityEngine;
  using UnityEngine.EventSystems;

  public class InputModuleSelector : MonoBehaviour
  {
    [SerializeField]
    private BaseInputModule standaloneInputModule;

    protected void Start()
    {
      this.standaloneInputModule.enabled = false;
      this.standaloneInputModule.enabled = true;
    }

    protected void OnValidate()
    {
      if (this.standaloneInputModule == null)
      {
        Debug.LogError(
          $"The variable '{nameof(this.standaloneInputModule)}' of  the component '{this.GetType().Name}' of the game object '{this.name}' has not been assigned.",
          this.gameObject);
      }
    }
  }
}
