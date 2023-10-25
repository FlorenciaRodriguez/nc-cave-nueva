#pragma warning disable 649
namespace Assets.UI.Splash.Scripts
{
  using Assets.UI.Common.Scripts;

  using UnityEngine;
  using UnityEngine.Events;

  public class SplashController : MonoBehaviour
  {
    private const string FadeOutTriggerName = "FadeOut";

    private Animator animator;

    private bool started;

    [SerializeField]
    private ProgressBarController progressBarController;

    public event UnityAction Shown;

    public event UnityAction Hidden;

    public static SplashController Singleton { get; private set; }

    public float Progress
    {
      get => this.progressBarController?.CurrentPercent ?? 0.0f;

      set
      {
        if (this.progressBarController != null)
        {
          this.progressBarController.CurrentPercent = value;
        }
      }
    }

    public void SetActive(bool active)
    {
      if (active)
      {
        this.gameObject.SetActive(true);
      }
      else
      {
        this.animator.SetTrigger(FadeOutTriggerName);
      }
    }

    protected void Awake()
    {
      if (Singleton != null)
      {
        // ReSharper disable once ArrangeStaticMemberQualifier
        GameObject.DestroyImmediate(this.gameObject);
        return;
      }

      // ReSharper disable once ArrangeStaticMemberQualifier
      GameObject.DontDestroyOnLoad(this.gameObject);

      Singleton = this;
      this.started = false;

      this.AwakeSingleton();
    }

    protected void Start()
    {
      if (Singleton == this)
      {
        this.OnEnableSingleton();
        this.started = true;
      }
    }

    protected void OnEnable()
    {
      if (Singleton == this && this.started)
      {
        this.OnEnableSingleton();
      }
    }

    protected void OnDisable()
    {
      if (Singleton == this)
      {
        this.OnDisableSingleton();
      }
    }

    protected void OnDestroy()
    {
      if (Singleton == this)
      {
        Singleton = null;
      }
    }

    protected virtual void OnValidate()
    {
      if (this.progressBarController == null)
      {
        Debug.LogError(
          $"The variable '{nameof(this.progressBarController)}' of the component '{this.GetType().Name}' of the game object '{this.name}' has not been assigned.",
          this.gameObject);
      }
    }

    protected virtual void AwakeSingleton()
    {
      this.animator = this.GetComponent<Animator>();
    }

    protected virtual void OnEnableSingleton()
    {
      this.animator.ResetTrigger(FadeOutTriggerName);

      this.Shown?.Invoke();
    }

    protected virtual void OnDisableSingleton()
    {
      this.animator.ResetTrigger(FadeOutTriggerName);

      this.Hidden?.Invoke();
    }

    protected void FadeOutAnimationFinished()
    {
      this.gameObject.SetActive(false);
    }
  }
}
