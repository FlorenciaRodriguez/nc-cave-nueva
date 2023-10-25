#pragma warning disable 649
namespace Assets.Scripts.ApplicationStateMachine
{
  using System;

  using Assets.Scripts.Adic.Extensions;
  using Assets.Scripts.ApplicationStateMachine.Event.Args;
  using Assets.Scripts.ApplicationStateMachine.StateActions;

  using MediaLab.Adic.Framework.Attributes;
  using MediaLab.Adic.Framework.Container;
  using MediaLab.StateMachine.ControlAction.StateMachine;

  public class StateMachineManager
  {
    [Inject]
    private IInjectionContainer injectionContainer;

    public event EventHandler<StateMachineEventArgs> StateChanged;

    public enum StateEventEnum
    {
      None,
      Initializing,
      Started,
      Loading,
      Waiting,
      Playing,
      Paused,
      ShowingReport,
      Resetting,
      ShowingError,
      ResettingFinishedExercise,
      Calibrating
    }

    public StateEventEnum LastStateEventFired { get; private set; } = StateEventEnum.None;

    public ApplicationStateMachine ApplicationStateMachine { get; private set; }

    public void StartStateMachine()
    {
      this.ApplicationStateMachine = new ApplicationStateMachine();
      this.SetupBindingStateActions();

      this.injectionContainer.Resolve<InitializingStateAction>().Start();
    }

    public void FireStateChanged(StateMachineEventArgs stateMachineEventArgs)
    {
      this.LastStateEventFired = stateMachineEventArgs.StateEventEnum;
      this.StateChanged?.Invoke(this, stateMachineEventArgs);
    }

    private void SetupBindingStateActions()
    {
      this.injectionContainer.UpdateBind<ExecutingStateAction>().ToSingleton();
      this.injectionContainer.UpdateBind<InitializingStateAction>().ToSingleton();
      this.injectionContainer.UpdateBind<LoadingStateAction>().ToSingleton();
      this.injectionContainer.UpdateBind<ResettingFinishedExerciseStateAction>().ToSingleton();
      this.injectionContainer.UpdateBind<ResettingStateAction>().ToSingleton();
      this.injectionContainer.UpdateBind<ShowingErrorStateAction>().ToSingleton();
      this.injectionContainer.UpdateBind<ShowingReportStateAction>().ToSingleton();
      this.injectionContainer.UpdateBind<StartedStateAction>().ToSingleton();
      this.injectionContainer.UpdateBind<WaitingStateAction>().ToSingleton();
    }
  }
}