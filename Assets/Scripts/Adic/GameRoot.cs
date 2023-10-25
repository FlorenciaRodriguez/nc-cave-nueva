namespace Assets.Scripts.Adic
{
  using System.Diagnostics.CodeAnalysis;

  using Assets.Scripts.Adic.ApplicationConfigurer;
  using Assets.Scripts.ApplicationStateMachine;
  using Assets.Scripts.Helpers;

  using MediaLab.Adic.Extensions.BindingCaller;
  using MediaLab.Adic.Extensions.ContextRoot;
  using MediaLab.Adic.Extensions.EventCaller;
  using MediaLab.Adic.Extensions.InitializableCaller;
  using MediaLab.Adic.Extensions.UnityBinding;
  using MediaLab.Adic.Framework.Container;
  using MediaLab.Adic.Framework.Injection;
  using MediaLab.Common.Helpers;
  using MediaLab.Common.Visual.Globalization;
  using MediaLab.LogManager;

  using UnityEngine;

  [SuppressMessage("ReSharper", "StyleCop.SA1126", Justification = "Reviewed. Suppression is OK here.")]
  public class GameRoot : ContextRoot
  {
    private ApplicationModeConfigurer applicationModeConfigurer;

    public IInjectionContainer Container { get; private set; }

    public override void SetupContainers()
    {
      GlobalizationHelper.SetAppLanguage(MediaLab.Simpa.Settings.Properties.Settings.Default.CultureInfo);

      UnityExceptionHelper.SetAction((message, stackTrace) =>
        {
          Log.Fatal($"{message}\n{stackTrace}");
          Log.Fatal("An exception has occurred. Application closed");

          ApplicationHelper.Exit();
        });

      if (ApplicationHelper.ApplicationCameraMode == ApplicationHelper.ApplicationCameraModeEnum.Cave)
      {
        if (Display.displays != null)
        {
          foreach (var display in Display.displays)
          {
            display.Activate();
          }
        }
      }

      this.Container = this.AddContainer(new InjectionContainer(ResolutionMode.ReturnNull), false);

      this.Container.RegisterExtension<UnityBindingContainerExtension>();
      this.Container.RegisterExtension<EventCallerContainerExtension>();
      this.Container.RegisterExtension<InitializableCallerContainerExtension>();
      this.Container.RegisterExtension<BindCallerContainerExtension>();

      this.SetupBindings();
    }

    public override void Init()
    {
#if !UNITY_EDITOR
      if (ApplicationHelper.ApplicationMode == ApplicationHelper.ApplicationModeEnum.Production)
      {
        var windowOnTopMost = new UnityEngine.GameObject(nameof(WindowOnTopMost.WindowOnTopMostController)).AddComponent<WindowOnTopMost.WindowOnTopMostController>();
        windowOnTopMost.WaitTime = MediaLab.Simpa.Settings.Properties.Settings.Default.WindowOnTopMostWaitTime;
        windowOnTopMost.MaximumTime = MediaLab.Simpa.Settings.Properties.Settings.Default.WindowOnTopMostMaximumTime;
        windowOnTopMost.IsStartCoroutine = windowOnTopMost.MaximumTime > 0;
      }
#endif

      this.Container.Resolve<StateMachineManager>()?.StartStateMachine();
    }

    private void SetupBindings()
    {
      this.applicationModeConfigurer =
        ApplicationHelper.ApplicationMode == ApplicationHelper.ApplicationModeEnum.Sandbox
          ? new SandboxModeConfigurer()
          : new ProductionModeConfigurer() as ApplicationModeConfigurer;

      this.applicationModeConfigurer.SetupBindings(this.Container);
    }
  }
}