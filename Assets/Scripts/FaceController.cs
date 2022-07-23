namespace catwars {

using UnityEngine;

public class FaceController : DraggableItem {
  private CatState cat;

  public Sprite[] faceSprites;

  public void Init (CatState cat) {
    this.cat = cat;
    image.sprite = faceSprites[cat.faceId];
  }

  protected override object DragState => cat;
}
}
