#pragma warning disable 649
namespace Assets.Scripts.Communication
{
  using System.Collections.Generic;

  using Assets.Scripts.Executors;
  using Assets.Scripts.Helpers;

  using MediaLab.Adic.ClientCommunication;
  using MediaLab.Adic.ClientCommunication.Statistics;
  using MediaLab.Adic.Extensions.EventCaller;
  using MediaLab.Adic.Extensions.InitializableCaller;
  using MediaLab.Adic.Framework.Attributes;
  using MediaLab.Adic.Framework.Container;
  using MediaLab.NetworkEntities.Entities;
  using MediaLab.NetworkEntities.Executors;
  using MediaLab.NetworkEntities.Helpers;
  using MediaLab.NetworkEntities.Simpa.Exercises;
  using MediaLab.Simpa.Settings.Properties;

  using UnityEngine;

  public class CommunicationManager : IInitializable, IUpdatable, IQuitable
  {
    [Inject]
    private IInjectionContainer injectionContainer;

    private ProxyClientCommunication proxyClientCommunication;

    public CommunicationStatistics CommunicationStatistics => this.proxyClientCommunication?.CommunicationStatistics;

    public void SendMessage(JsonEntity jsonEntity)
    {
      this.proxyClientCommunication?.SendMessage(jsonEntity);
    }

    public void ClearBuffers()
    {
      this.proxyClientCommunication?.ClearBuffers();
    }

    void IInitializable.Init()
    {
      this.RegisterJsonEntities();
      this.proxyClientCommunication = this.CreateClientCommunication();
      this.ConnectAndConfig(this.proxyClientCommunication);
    }

    void IUpdatable.Update()
    {
      this.proxyClientCommunication?.Update();
    }

    void IQuitable.OnApplicationQuit()
    {
      this.proxyClientCommunication?.Disconnect();
    }

    protected virtual ProxyClientCommunication CreateClientCommunication()
    {
      return new ProxyClientCommunication();
    }

    protected virtual void ConnectAndConfig(ProxyClientCommunication clientCommunication)
    {
      var visualClientName = Settings.Default.ClientNamePrefix + "1";
      var instructorClientName = Settings.Default.InstructorListenerPrefix + "1";
      var controlClientName = Settings.Default.ControlListenerPrefix + "1";

      var listeners = new List<string> { instructorClientName, controlClientName };

      Debug.Log(
        $"Connecting client '{visualClientName}' to server '{Settings.Default.ServerIp}:{Settings.Default.ServerPort}'. Listeners: '{instructorClientName}, {controlClientName}'");

      if (clientCommunication.ConnectAndConfig(Settings.Default.ServerIp, Settings.Default.ServerPort, visualClientName, listeners, Settings.Default.ConnectionMode))
      {
        Debug.Log($"Client '{visualClientName}' successfully connected to server");

        clientCommunication.OnClientDataReceived += this.HandleClientDataReceived;
        clientCommunication.OnClientDisconnectedFromServer += this.HandleClientDisconnectedFromClient;
      }
      else
      {
        Debug.LogWarning($"The client '{visualClientName}' could not connect to the server. Application closed");
        ApplicationHelper.Exit();
      }
    }

    protected virtual void RegisterJsonEntities()
    {
      // All entities that are in the ControlActionEntity assembly are registered
      JsonHelper.AddEntityAssembly(typeof(ControlActionEntity));
      JsonHelper.AddEntityAssembly(typeof(ExerciseEntity));

      // All executors that are in the ControlActionEntityExecutor assembly are registered
      ExecutorHelper.AddExecutorAssembly(typeof(ControlActionEntityExecutor));
      ExecutorHelper.AddExecutorAssembly(typeof(ExerciseEntity));
    }

    protected virtual void HandleClientDataReceived(object sender, object data)
    {
      JsonEntity jsonEntity = (JsonEntity)data;
      Executor executor = jsonEntity.GetExecutor();
      if (executor != null)
      {
        this.injectionContainer.Inject(executor); // Inject dependencies
        executor.Execute();
      }
      else
      {
        Debug.LogError($"An executor was not found for the entity '{jsonEntity.GetType()}'");
      }
    }

    protected virtual void HandleClientDisconnectedFromClient(object sender, string hostName)
    {
      Debug.LogWarning("Disconnected from server. Application closed");
      ApplicationHelper.Exit();
    }
  }
}
