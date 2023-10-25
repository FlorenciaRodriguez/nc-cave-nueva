namespace Assets.Scripts.ApplicationStateMachine.Event.Args
{
  using System;

  public class StateMachineEventArgs : EventArgs
  {
    public StateMachineEventArgs(StateMachineManager.StateEventEnum stateEventEnum)
    {
      this.StateEventEnum = stateEventEnum;
    }

    public StateMachineManager.StateEventEnum StateEventEnum { get; }
  }
}
