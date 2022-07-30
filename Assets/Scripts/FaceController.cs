namespace catwars {

public class FaceController : DraggableItem {
  private CatState cat;

  public Icons icons;

  public void Init (CatState cat) {
    this.cat = cat;
    image.sprite = icons.faces[cat.faceId];
  }

  protected override object DragState => cat;
}
}
