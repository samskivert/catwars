namespace catwars {

using UnityEngine;
using UnityEngine.UI;

public class FaceController : MonoBehaviour {

  public Sprite[] faceSprites;
  public Image faceImage;

  public void Show (int faceId) {
    faceImage.sprite = faceSprites[faceId];
  }
}

}
