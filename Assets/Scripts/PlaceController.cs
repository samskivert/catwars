namespace catwars {

using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

using React;

public class PlaceController : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler {
  private ClanController clan;

  public Place place;
  public Image image;

  private void Start () {
    clan = GetComponentInParent<ClanController>();
    clan.dragInfo.OnValue(pair => {
      var (cat, hov) = pair;
      if (cat != null && hov == this) image.color = Color.red;
      else image.color = Color.white;
    });
  }

  public void OnPointerEnter (PointerEventData eventData) {
    clan.hoveredPlace.Update(this);
  }

  public void OnPointerExit (PointerEventData eventData) {
    clan.hoveredPlace.UpdateVia(hp => hp == this ? null : hp);
  }
}

}
