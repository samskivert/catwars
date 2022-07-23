namespace catwars {

using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

using Util;

public class DraggableItem : MonoBehaviour,  IBeginDragHandler, IDragHandler, IEndDragHandler {
  private bool active;
  private ClanController clan;

  private RectTransform parentCanvas;
  private Vector2 neutralPos;
  private Vector2 dragStart;

  public Image image;
  public RectTransform dragRect;

  private void Start () {
    clan = GetComponentInParent<ClanController>();
    parentCanvas = GetComponentInParent<Canvas>().GetComponent<RectTransform>();
  }

  public void SetActive (bool active) {
    this.active = active;
    image.SetAlpha(active ? 1f : 0.25f);
  }

  public void OnBeginDrag (PointerEventData data) {
    if (!active) return;
    dragRect.transform.SetParent(parentCanvas.transform);
    neutralPos = dragRect.localPosition;
    dragStart = dragRect.localPosition;
    image.raycastTarget = false;
    RectTransformUtility.ScreenPointToLocalPointInRectangle(
      parentCanvas, data.position, data.pressEventCamera, out dragStart);
    clan.draggedItem.Update(DragState);
  }

  public void OnDrag (PointerEventData data) {
    if (!active) return;
    if (RectTransformUtility.ScreenPointToLocalPointInRectangle(
      parentCanvas, data.position, data.pressEventCamera, out var dragPos)) {
      var offsetToOriginal = dragPos - dragStart;
      dragRect.localPosition = neutralPos + offsetToOriginal;
    }
  }

  public void OnEndDrag (PointerEventData eventData) {
    if (!active) return;
    dragRect.localPosition = neutralPos;
    dragRect.transform.SetParent(transform);
    clan.draggedItem.Update(null);
    image.raycastTarget = true;
  }

  protected virtual object DragState => null;
}
}
