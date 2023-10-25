namespace Assets.Scripts.Adic.ApplicationConfigurer
{
  using Assets.Scripts.ApplicationStateMachine;
  using Assets.Scripts.Apps.Bindings;
  using Assets.Scripts.Apps.LocationTour;
  using Assets.Scripts.Apps.Receivers;
  using Assets.Scripts.Communication;
  using Assets.Scripts.Exercise;
  using Assets.Scripts.Sandbox;
  using MediaLab.Adic.Framework.Container;

  using UnityEngine;

  public class ProductionModeConfigurer : ApplicationModeConfigurer
  {
    public override void SetupBindings(IInjectionContainer injectionContainer)
    {
      Debug.Log("Initializing Production Mode");

      injectionContainer.Bind<CommunicationManager>().ToSingleton();
      injectionContainer.Bind<StateMachineManager>().ToSingleton();

      injectionContainer.Bind<KeyStateMachine>().ToSingleton();
      
      injectionContainer.Bind<ExerciseManager>().ToSingleton();

      injectionContainer.Bind<EntityReceiverBindingSetup>().ToSingleton();
      injectionContainer.Resolve<EntityReceiverBindingSetup>().SetupBindings(injectionContainer);

      injectionContainer.Bind<AppBindingSetup>().ToSingleton();
      injectionContainer.Resolve<AppBindingSetup>().SetupBindings(injectionContainer);

      injectionContainer.Bind<LocationTourAutoStart>().ToSingleton();

#if !UNITY_EDITOR
      Cursor.visible = false;
#endif
    }
  }
}