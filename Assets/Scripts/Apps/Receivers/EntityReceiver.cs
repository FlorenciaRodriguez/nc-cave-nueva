namespace Assets.Scripts.Apps.Receivers
{
  using MediaLab.NetworkEntities.Entities;

  using UnityEngine.Events;

  public class EntityReceiver<T> where T : JsonEntity
  {
    private readonly UnityEvent<T> entityEvent = new UnityEvent<T>();

    public void AddListener(UnityAction<T> listener)
    {
      this.entityEvent.AddListener(listener);
    }

    public void RemoveListener(UnityAction<T> listener)
    {
      this.entityEvent.RemoveListener(listener);
    }

    public void OnEntityReceived(T appViewEntity)
    {
      this.entityEvent.Invoke(appViewEntity);
    }
  }
}
