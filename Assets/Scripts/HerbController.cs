namespace catwars {

using UnityEngine;
using TMPro;

public class HerbController : DraggableItem {

  public Sprite[] herbSprites;
  public TMP_Text herbCount;

  public void SetHerb (Herb herb) {
    image.sprite = herbSprites[(int)herb];
  }

  public void SetCount (int count) {
    herbCount.text = count.ToString();
  }
}

}
