#pragma warning disable 649
namespace Assets.UI.Common.Scripts
{
  using System;

  using UnityEngine;
  using UnityEngine.UI;

  [Serializable]
  [ExecuteInEditMode]
  public class ProgressBarController : MonoBehaviour
  {
    [SerializeField]
    private Image loadingBarImage;

    [SerializeField]
    [HideInInspector]
    private float currentPercent;

    public float CurrentPercent
    {
      get
      {
        return this.currentPercent;
      }

      set
      {
        this.currentPercent = Mathf.Clamp01(value);

        this.SetProgress();
      }
    }

    public Image LoadingBarImage => this.loadingBarImage;

    protected void Start()
    {
      this.SetProgress();
    }

    private void SetProgress()
    {
      if (this.loadingBarImage != null)
      {
        this.loadingBarImage.fillAmount = this.currentPercent;
      }
    }
  }
}