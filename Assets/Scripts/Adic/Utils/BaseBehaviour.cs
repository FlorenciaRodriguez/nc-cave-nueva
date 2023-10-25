namespace Assets.Scripts.Adic.Utils
{
  using MediaLab.Adic.Extensions.MonoInjection;

  using UnityEngine;

  public abstract class BaseBehaviour : MonoBehaviour
  {
    protected virtual void Start()
    {
      this.Inject();
    }
  }
}