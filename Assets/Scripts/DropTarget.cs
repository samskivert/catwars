namespace catwars {

using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

using React;

public class DropTarget : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler {
  private ClanController clan;

  public Image image;

  private void Start () {
    clan = GetComponentInParent<ClanController>();
    var hoveredV = clan.dragInfo.Map(
      pair => pair.Item1 != null && DropState.Equals(pair.Item2) && CanDrop(clan.game.phase.current, pair.Item1));

    Color ocolor = image.color;
    hoveredV.OnEmit(hovered => {
      if (hovered) {
        ocolor = image.color;
        image.color = Color.red;
      } else image.color = ocolor;
    });
  }

  public virtual bool CanDrop (Phase phase, object dragState) => false;

  public void OnPointerEnter (PointerEventData eventData) {
    clan.hoveredTarget.Update(DropState);
  }

  public void OnPointerExit (PointerEventData eventData) {
    if (DropState.Equals(clan.hoveredTarget.current)) clan.hoveredTarget.Update(null);
  }

  protected virtual object DropState => this;
}

}
