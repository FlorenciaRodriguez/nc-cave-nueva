namespace Assets.TransitionAnimation.Scripts
{
  using UnityEngine;
  using UnityEngine.Events;

  public abstract class BaseTransitionAnimation : MonoBehaviour
  {
    public event UnityAction<BaseTransitionAnimation, CameraTransform, CameraTransform> TransitionStarted;

    public event UnityAction<BaseTransitionAnimation, CameraTransform, CameraTransform> TransitionFinished;

    public event UnityAction<BaseTransitionAnimation> TransitionStopped;

    public bool IsExecuting { get; private set; }

    public abstract void StartAnimation(CameraTransform from, CameraTransform to);

    public abstract void StopAnimation();

    protected void OnTransitionStarted(CameraTransform from, CameraTransform to)
    {
      this.IsExecuting = true;
      this.TransitionStarted?.Invoke(this, from, to);
    }

    protected void OnTransitionFinished(CameraTransform from, CameraTransform to)
    {
      this.IsExecuting = false;
      this.TransitionFinished?.Invoke(this, from, to);
    }

    protected void OnTransitionStopped()
    {
      this.IsExecuting = false;
      this.TransitionStopped?.Invoke(this);
    }

    public struct CameraTransform
    {
      public Vector3 Position;

      public Quaternion Rotation;
    }
  }
}