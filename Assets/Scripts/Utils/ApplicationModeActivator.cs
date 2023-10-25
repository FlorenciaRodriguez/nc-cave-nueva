namespace Assets.Scripts.Utils
{
  using Assets.Scripts.Helpers;

  using UnityEngine;

  public class ApplicationModeActivator : MonoBehaviour
  {
    [SerializeField]
    private ApplicationHelper.ApplicationModeEnum mode;

    protected void Awake()
    {
      this.gameObject.SetActive(ApplicationHelper.ApplicationMode == this.mode);
    }
  }
}
