namespace Assets.Scripts.Apps.LocationTour
{
  using Assets.Scripts.ApplicationStateMachine;
  using Assets.Scripts.ApplicationStateMachine.Event.Args;
  using Assets.Scripts.Exercise.Helpers;

  using MediaLab.Adic.Extensions.BindingCaller;
  using MediaLab.Adic.Framework.Attributes;
  using MediaLab.Adic.Framework.Container;
  using MediaLab.NetworkEntities;
  using MediaLab.NetworkEntities.Helpers;
  using MediaLab.NetworkEntities.Simpa.Instruction;

  public class LocationTourAutoStart : IBindContainer
  {
    [Inject]
    private StateMachineManager stateMachineManager;

    [Inject]
    private IInjectionContainer injectionContainer;

    public void BindOnContainer(IInjectionContainer container)
    {
      this.stateMachineManager.StateChanged += this.HandleStateChanged;
    }

    private void HandleStateChanged(object sender, StateMachineEventArgs stateMachineEventArgs)
    {
      if (stateMachineEventArgs.StateEventEnum == StateMachineManager.StateEventEnum.Started)
      {
        this.LoadExercise();
      }
    }

    private void LoadExercise()
    {
      var exerciseEntity = ExerciseMockHelper.LoadExerciseEntity(1);
      if (exerciseEntity != null)
      {
        LoadControlActionEntity loadControlActionEntity = new LoadControlActionEntity
        {
          ExerciseEntity = exerciseEntity,
          ControlAction = Constants.ControlActionEnum.Load,
          Status = Constants.ControlActionStatusEnum.Pending
        };
    
        var executor = loadControlActionEntity.GetExecutor();
        this.injectionContainer.Inject(executor);
        executor.Execute();
      }
    }
  }
}
