namespace catwars {

using UnityEngine;

public class PreyController : DraggableItem {

  [EnumArray(typeof(Prey))] public Sprite[] sprites;

  public Prey prey { get; set ; }

  public void SetPrey (Prey prey) {
    this.prey = prey;
    image.sprite = sprites[(int)prey];
  }

  protected override object DragState => prey;
}

}
