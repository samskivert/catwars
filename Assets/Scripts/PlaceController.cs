namespace catwars {

public class PlaceController : DropTarget {

  public Place place;

  protected override object DropState => place;
}

}
