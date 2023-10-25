namespace Assets.UI.Common.Scripts
{
  using UnityEngine;
  using UnityEngine.Events;
  using UnityEngine.EventSystems;

  [RequireComponent(typeof(RectTransform))]
  public class PanelPointerHandler : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
  {
    public event UnityAction<PanelPointerHandler> PointerEnter; 

    public event UnityAction<PanelPointerHandler> PointerExit; 

    void IPointerEnterHandler.OnPointerEnter(PointerEventData eventData)
    {
        this.PointerEnter?.Invoke(this);
    }

    void IPointerExitHandler.OnPointerExit(PointerEventData eventData)
    {
      this.PointerExit?.Invoke(this);
    }
  }
}
