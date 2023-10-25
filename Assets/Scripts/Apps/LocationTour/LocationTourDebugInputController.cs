#pragma warning disable 649
namespace Assets.Scripts.Apps.LocationTour
{
  using MediaLab.Adic.Extensions.EventCaller;
  using MediaLab.Adic.Framework.Attributes;
  using MediaLab.Adic.Framework.Container;
  using MediaLab.Common.Serializables;
  using MediaLab.NetworkEntities.Helpers;
  using MediaLab.NetworkEntities.Simpa;
  using MediaLab.NetworkEntities.Simpa.Configuration;
  using System;
  using UnityEngine;

  using Random = UnityEngine.Random;

  public class LocationTourDebugInputController : IUpdatable
  {
    private static readonly int MomentOfTheDayCount = Enum.GetValues(typeof(Constants.MomentsOfTheDayEnum)).Length;

    [Inject]
    private IInjectionContainer injectionContainer;

    [Inject]
    private LocationTourManager locationTourManager;

    void IUpdatable.Update()
    {
      if (Input.GetKeyDown(KeyCode.F5))
      {
        var momentOfDay = new MomentOfTheDayEntity { MomentOfTheDay = this.NextMomentsOfTheDay(this.locationTourManager.MomentOfTheDay) };

        this.locationTourManager.HandleMomentOfTheDayEntityReceived(momentOfDay);
      }
      else if (Input.GetKeyDown(KeyCode.F6))
      {
        var appViewEntity = new AppViewEntity { AppView = Constants.AppViewEnum.Drone };

        var executor = appViewEntity.GetExecutor();
        this.injectionContainer.Inject(executor);
        executor.Execute();
      }
      else if (Input.GetKeyDown(KeyCode.F7))
      {
        var locationGuyEntity = new LocationGuyEntity { ScreenPoint = new SerializableVector2(Random.Range(0.25f, 0.75f), Random.Range(0.25f, 0.75f)) };

        var executor = locationGuyEntity.GetExecutor();
        this.injectionContainer.Inject(executor);
        executor.Execute();
      }
    }

    private Constants.MomentsOfTheDayEnum NextMomentsOfTheDay(Constants.MomentsOfTheDayEnum current)
    {
      return (Constants.MomentsOfTheDayEnum)(((int)current + 1) % MomentOfTheDayCount);
    }
  }
}
