namespace Assets.Scripts.SandBox
{
  using MediaLab.Adic.ClientCommunication;
  using MediaLab.NetworkEntities.Entities;

  public class StandAloneClientCommunication : ProxyClientCommunication
  {
    private readonly MockInstructor mockInstructor;

    public StandAloneClientCommunication()
    {
      this.mockInstructor = new MockInstructor(this);
    }

    public void HandleMockMessage(JsonEntity jsonEntity)
    {
      this.HandleClientDataReceivedJsonEntity(this, jsonEntity);
    }

    public override bool SendMessage(JsonEntity jsonEntity)
    {
      this.mockInstructor.HandleMessageFromVisual(jsonEntity);
      return true;
    }

    public override void Update()
    {
      base.Update();
      this.mockInstructor.Update();
    }
  }
}
