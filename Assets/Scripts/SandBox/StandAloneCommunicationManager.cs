#pragma warning disable 649
namespace Assets.Scripts.SandBox
{
  using Assets.Scripts.Communication;
  using MediaLab.Adic.ClientCommunication;

  public class StandAloneCommunicationManager : CommunicationManager
  {
    protected override ProxyClientCommunication CreateClientCommunication()
    {
      return new StandAloneClientCommunication();
    }

    protected override void ConnectAndConfig(ProxyClientCommunication clientCommunication)
    {
      clientCommunication.OnClientDataReceived += this.HandleClientDataReceived;
    }
  }
}
