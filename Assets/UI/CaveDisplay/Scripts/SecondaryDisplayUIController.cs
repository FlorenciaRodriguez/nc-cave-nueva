#pragma warning disable 649
namespace Assets.UI.CaveDisplay.Scripts
{
  using Assets.Scripts.Helpers;

  using TMPro;
  using UnityEngine;

  [RequireComponent(typeof(Canvas))]
  public class SecondaryDisplayUIController : MonoBehaviour
  {
    [SerializeField]
    private TextMeshProUGUI label;

    private Canvas canvas;

    protected void Awake()
    {
      this.canvas = this.GetComponent<Canvas>();
    }

    protected void OnEnable()
    {
      if (ApplicationHelper.ApplicationCameraMode == ApplicationHelper.ApplicationCameraModeEnum.Cave)
      {
        this.label.text = (this.canvas.targetDisplay + 1).ToString();
      }
      else
      {
        this.canvas.enabled = false;
      }
    }

    protected void OnDisable()
    {

    }

    protected void OnValidate()
    {
      if (this.label == null)
      {
        Debug.LogError(
          $"The variable '{nameof(this.label)}' of '{this.GetType().Name}' of the game object '{this.name}' has not been assigned.",
          this.gameObject);
      }
    }
  }
}
