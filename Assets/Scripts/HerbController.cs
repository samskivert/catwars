namespace catwars {

using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HerbController : MonoBehaviour {

  public Sprite[] herbSprites;
  public Image herbImage;
  public TMP_Text herbCount;

  public void SetHerb (int herbId) {
    herbImage.sprite = herbSprites[herbId];
  }

  public void SetCount (int count) {
    herbCount.text = count.ToString();
  }
}

}
