namespace catwars {

using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DropTarget : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler {
  private ClanController clan;

  public Image image;

  private void Start () {
    clan = GetComponentInParent<ClanController>();
    clan.dragInfo.OnValue(pair => {
      var (item, hov) = pair;
      if (item != null && DropState.Equals(hov)) image.color = Color.red;
      else image.color = Color.white;
    });
  }

  public void OnPointerEnter (PointerEventData eventData) {
    clan.hoveredTarget.Update(DropState);
  }

  public void OnPointerExit (PointerEventData eventData) {
    if (DropState.Equals(clan.hoveredTarget.current)) clan.hoveredTarget.Update(null);
  }

  protected virtual object DropState => this;
}

}
