#pragma warning disable 649
namespace Assets.UI.Common.Scripts
{
  using Assets.UI.Common.Scripts.Helpers;

  using TMPro;

  using UnityEngine;

  [RequireComponent(typeof(TextMeshProUGUI))]
  public class TextResex : MonoBehaviour
  {
    [SerializeField]
    [HideInInspector]
    private ResexsHelper.ResexTypeEnum resexType;

    [SerializeField]
    [HideInInspector]
    private string resourceName;

    private TextMeshProUGUI text;

    public ResexsHelper.ResexTypeEnum ResexType
    {
      get
      {
        return this.resexType;
      }

      set
      {
        this.resexType = value;
      }
    }

    public string ResourceName
    {
      get
      {
        return this.resourceName;
      }

      set
      {
        this.resourceName = value;
      }
    }

    public void UpdateText()
    {
      this.text.text = ResexsHelper.GetResourceStringValue(this.resourceName, this.resexType);
    }

    protected void Awake()
    {
      this.text = this.GetComponent<TextMeshProUGUI>();
    }

    protected void Start()
    {
      this.UpdateText();
    }

    protected void OnValidate()
    {
      if (Application.isPlaying && this.text != null)
      {
        this.UpdateText();
      }
    }
  }
}