namespace Assets.Scripts.Input
{
  using UnityEngine;

  public abstract class ApplicationInputBehaviour : MonoBehaviour
  {
    private ApplicationInputActions applicationInputActions;

    protected virtual void Awake()
    {
      this.applicationInputActions = new ApplicationInputActions();
    }

    protected virtual void OnEnable()
    {
      this.Subscribe(this.applicationInputActions);
      this.applicationInputActions.Enable();
    }

    protected virtual void OnDisable()
    {
      this.Unsubscribe(this.applicationInputActions);
      this.applicationInputActions.Disable();
    }

    protected virtual void OnDestroy()
    {
      this.applicationInputActions.Dispose();
    }

    protected abstract void Subscribe(ApplicationInputActions inputActions);

    protected abstract void Unsubscribe(ApplicationInputActions inputActions);
  }
}
