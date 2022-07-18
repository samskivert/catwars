namespace catwars {

using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

using Util;

public class FaceController : MonoBehaviour,  IBeginDragHandler, IDragHandler, IEndDragHandler {
  private CatState cat;
  private ClanController clan;
  private bool active;

  private RectTransform parentCanvas;
  private Vector2 neutralPos;
  private Vector2 dragStart;

  public Sprite[] faceSprites;
  public Image faceImage;
  public RectTransform faceDragRect;

  private void Start () {
    clan = GetComponentInParent<ClanController>();
    parentCanvas = GetComponentInParent<Canvas>().GetComponent<RectTransform>();
  }

  public void Init (CatState cat) {
    this.cat = cat;
    faceImage.sprite = faceSprites[cat.faceId];
  }

  public void SetActive (bool active) {
    this.active = active;
    faceImage.SetAlpha(active ? 1f : 0.5f);
  }

  public void OnBeginDrag (PointerEventData data) {
    if (!active) return;
    faceDragRect.transform.SetParent(parentCanvas.transform);
    neutralPos = faceDragRect.localPosition;
    dragStart = faceDragRect.localPosition;
    faceImage.raycastTarget = false;
    RectTransformUtility.ScreenPointToLocalPointInRectangle(
      parentCanvas, data.position, data.pressEventCamera, out dragStart);
    clan.draggedCat.Update(cat);
  }

  public void OnDrag (PointerEventData data) {
    if (RectTransformUtility.ScreenPointToLocalPointInRectangle(
      parentCanvas, data.position, data.pressEventCamera, out var dragPos)) {
      var offsetToOriginal = dragPos - dragStart;
      faceDragRect.localPosition = neutralPos + offsetToOriginal;
    }
  }

  public void OnEndDrag (PointerEventData eventData) {
    faceDragRect.localPosition = neutralPos;
    faceDragRect.transform.SetParent(transform);
    clan.draggedCat.Update(null);
    faceImage.raycastTarget = true;
  }
}

}
