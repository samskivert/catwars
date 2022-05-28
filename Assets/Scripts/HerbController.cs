namespace catwars {

using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HerbController : MonoBehaviour {

  public Sprite[] herbSprites;
  public Image herbImage;
  public TMP_Text herbCount;

  public void Show (int herbId, int count) {
    herbImage.sprite = herbSprites[herbId];
    herbCount.text = count.ToString();
  }
}

}
