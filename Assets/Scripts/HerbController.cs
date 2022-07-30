namespace catwars {

using TMPro;

public class HerbController : DraggableItem {

  public Icons icons;
  public TMP_Text herbCount;

  public void SetHerb (Herb herb) {
    image.sprite = icons.Herb(herb);
  }

  public void SetCount (int count) {
    herbCount.text = count.ToString();
  }
}
}
