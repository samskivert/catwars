namespace catwars {

public class PlaceController : DropTarget {

  public Place place;

  public override bool CanDrop (Phase phase, object dragState) =>
    phase == Phase.Hunt && dragState is CatState;

  protected override object DropState => place;
}

}
