#pragma warning disable 649
namespace Assets.Scripts.Apps.LocationTour.Components
{
  using System;

  using Assets.Scripts.Environment;

  using MediaLab.Adic.Extensions.BindingCaller;
  using MediaLab.Adic.Framework.Attributes;
  using MediaLab.Adic.Framework.Container;
  using MediaLab.NetworkEntities.Simpa;

  public class MomentOfTheDayController : IBindContainer, IUnbindContainer
  {
    [Inject]
    private LocationTourManager locationTourManager;

    [Inject]
    private EnvironmentManager environmentManager;

    void IBindContainer.BindOnContainer(IInjectionContainer container)
    {
      if (this.locationTourManager != null)
      {
        this.locationTourManager.MomentOfTheDayChanged += this.HandleMomentOfTheDayChanged;
        this.ConfigureMomentOfDay(this.locationTourManager.MomentOfTheDay);
      }
      else
      {
        throw new Exception($"The reference '{nameof(LocationTourManager)}' of  the class '{this.GetType().Name}' has not been assigned");
      }

      if (this.environmentManager == null)
      {
        throw new Exception($"The reference '{nameof(EnvironmentManager)}' of  the class '{this.GetType().Name}' has not been assigned");
      }
    }

    void IUnbindContainer.UnbindOnContainer(IInjectionContainer container)
    {
      if (this.locationTourManager != null)
      {
        this.locationTourManager.MomentOfTheDayChanged -= this.HandleMomentOfTheDayChanged;
        this.locationTourManager = null;
      }
    }

    private void HandleMomentOfTheDayChanged(LocationTourManager manager)
    {
      this.ConfigureMomentOfDay(this.locationTourManager.MomentOfTheDay);
    }

    private void ConfigureMomentOfDay(Constants.MomentsOfTheDayEnum momentOfTheDay)
    {
      this.environmentManager.SetEnvironmentCondition(momentOfTheDay);
    }
  }
}