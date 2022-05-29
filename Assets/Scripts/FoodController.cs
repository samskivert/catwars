namespace catwars {

using UnityEngine;
using TMPro;

public class FoodController : MonoBehaviour {

  public TMP_Text foodCount;

  public void Show (int count) {
    foodCount.text = count.ToString();
  }
}

}
