#pragma warning disable 649
namespace Assets.Scripts.Apps.Bindings
{
  using Assets.Scripts.Exercise;
  using Assets.Scripts.Helpers;

  using MediaLab.Adic.Extensions.BindingCaller;
  using MediaLab.Adic.Framework.Attributes;
  using MediaLab.Adic.Framework.Container;

  public abstract class AppBindingConfigurer : IBindContainer, IUnbindContainer
  {
    [Inject]
    private ExerciseManager exerciseManager;

    public abstract ApplicationHelper.AppTypeEnum AppType { get; }

    public abstract void AddBindings(IInjectionContainer container);

    public abstract void RemoveBindings(IInjectionContainer container);

    void IBindContainer.BindOnContainer(IInjectionContainer container)
    {
      this.exerciseManager.RegisterAppBinding(this.AppType, this);
    }

    void IUnbindContainer.UnbindOnContainer(IInjectionContainer container)
    {
      this.exerciseManager.UnregisterAppBinding(this.AppType, this);
    }
  }
}
