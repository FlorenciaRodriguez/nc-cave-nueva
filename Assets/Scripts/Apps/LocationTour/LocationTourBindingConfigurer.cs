namespace Assets.Scripts.Apps.LocationTour
{
  using Assets.Scripts.Adic.Extensions;
  using Assets.Scripts.Apps.Bindings;
  using Assets.Scripts.Apps.LocationTour.Components;
  using Assets.Scripts.Environment;
  using Assets.Scripts.Helpers;

  using MediaLab.Adic.Extensions.UnityBinding;
  using MediaLab.Adic.Framework.Container;

  public class LocationTourBindingConfigurer : AppBindingConfigurer
  {
    public override ApplicationHelper.AppTypeEnum AppType { get; } = ApplicationHelper.AppTypeEnum.LocationTour;

    public override void AddBindings(IInjectionContainer container)
    {
      container.UpdateBind<LocationTourManager>().ToSingleton();
      container.UpdateBind<LocationTourTransitionModeController>().ToGameObjectWithTag(LocationTourTransitionModeController.Tag);
      container.UpdateBind<EnvironmentManager>().ToGameObjectWithTag(EnvironmentManager.Tag);
      container.UpdateBind<MomentOfTheDayController>().ToSingleton();

      if (ApplicationHelper.ApplicationMode == ApplicationHelper.ApplicationModeEnum.Sandbox)
      {
        container.UpdateBind<LocationTourDebugInputController>().ToSingleton();
      }
    }

    public override void RemoveBindings(IInjectionContainer container)
    {
      container.Unbind<LocationTourManager>();
      container.Unbind<EnvironmentManager>();
      container.Unbind<MomentOfTheDayController>();

      if (ApplicationHelper.ApplicationMode == ApplicationHelper.ApplicationModeEnum.Sandbox)
      {
        container.Unbind<LocationTourDebugInputController>();
      }
    }
  }
}
