namespace catwars {

public class PreyController : DraggableItem {

  public Icons icons;
  public Prey prey { get; set ; }

  public void SetPrey (Prey prey) {
    this.prey = prey;
    image.sprite = icons.Prey(prey);
  }

  protected override object DragState => prey;
}

}
