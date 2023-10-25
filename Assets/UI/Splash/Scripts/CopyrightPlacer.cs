namespace Assets.UI.Splash.Scripts
{
  using MediaLab.Simpa.Settings.Helpers;

  using TMPro;

  using UnityEngine;

  [RequireComponent(typeof(TextMeshProUGUI))]
  public class CopyrightPlacer : MonoBehaviour
  {
    private TextMeshProUGUI textMeshPro;

    protected void Awake()
    {
      this.textMeshPro = this.GetComponent<TextMeshProUGUI>();
    }

    protected void Start()
    {
      this.textMeshPro.text = AssemblyVersionHelper.GetAssemblyVersion();
    }
  }
}
