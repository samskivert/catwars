namespace catwars {

using UnityEngine;
using UnityEngine.UI;

public class CatController : MonoBehaviour {
  private CatState cat;

  public FaceController face;
  public Image[] hungers;

  public void Init (CatState cat) {
    this.cat = cat;

    face.Init(cat);
    cat.hunger.OnValue(hunger => {
      hungers[0].gameObject.SetActive(hunger >= CatState.Hunger.Full);
      hungers[1].gameObject.SetActive(hunger >= CatState.Hunger.Hungry);
      hungers[2].gameObject.SetActive(hunger >= CatState.Hunger.Starving);
    });
  }
}

}
