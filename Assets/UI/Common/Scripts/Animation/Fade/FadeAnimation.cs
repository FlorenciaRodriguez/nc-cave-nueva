namespace Assets.UI.Common.Scripts.Animation.Fade
{
  using UnityEngine;
  using UnityEngine.Events;

  public abstract class FadeAnimation : MonoBehaviour
  {
    [SerializeField]
    private CanvasGroup targetCanvasGroup;

    [SerializeField]
    private RectTransform targetRectTransform;

    [Space]

    [SerializeField]
    private float animationTime = 1.0f;

    [SerializeField]
    private bool scaleAnimationEnabled;

    public event UnityAction<FadeAnimation> AnimationStarted; 

    public event UnityAction<FadeAnimation> AnimationFinished; 

    public CanvasGroup TargetCanvasGroup => this.targetCanvasGroup;

    public RectTransform TargetRectTransform => this.targetRectTransform;

    public float AnimationTime
    {
      get => this.animationTime;
      set => this.animationTime = Mathf.Max(0, value);
    }

    public bool IsScaleAnimationEnabled
    {
      get => this.scaleAnimationEnabled;
      set => this.scaleAnimationEnabled = value;
    }

    public bool IsExecuting { get; private set; }

    public abstract void StartAnimation();

    public abstract void StopAnimation();

    public abstract void ForceInitialState();

    public abstract void ForceFinalState();

    protected void OnValidate()
    {
      if (this.targetCanvasGroup == null)
      {
        Debug.LogError(
          $"The variable '{nameof(this.targetCanvasGroup)}' of  the component '{this.GetType().Name}' of the game object '{this.name}' has not been assigned.",
          this.gameObject);
      }

      if (this.targetRectTransform == null)
      {
        Debug.LogError(
          $"The variable '{nameof(this.targetRectTransform)}' of  the component '{this.GetType().Name}' of the game object '{this.name}' has not been assigned.",
          this.gameObject);
      }

      this.animationTime = Mathf.Max(0, this.animationTime);
    }

    protected void OnAnimationStarted()
    {
      if (!this.IsExecuting)
      {
        this.IsExecuting = true;
        this.AnimationStarted?.Invoke(this);
      }
    }

    protected void OnAnimationFinished()
    {
      if (this.IsExecuting)
      {
        this.IsExecuting = false;
        this.AnimationFinished?.Invoke(this);
      }
    }
  }
}
