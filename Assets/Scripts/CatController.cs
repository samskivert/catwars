namespace catwars {

using UnityEngine;
using UnityEngine.UI;

public class CatController : DropTarget {
  private CatState cat;

  public Icons icons;
  public FaceController face;
  public Image[] hungers;
  public Image type;

  public void Init (CatState cat) {
    this.cat = cat;

    face.Init(cat);
    cat.hunger.OnValue(hunger => {
      hungers[0].gameObject.SetActive(hunger >= CatState.Hunger.Full);
      hungers[1].gameObject.SetActive(hunger >= CatState.Hunger.Hungry);
      hungers[2].gameObject.SetActive(hunger >= CatState.Hunger.Starving);
    });

    cat.acted.OnValue(acted => face.SetActive(!acted));

    switch (cat.role.current) {
    case CatState.Role.Leader: ShowTypeIcon(icons.crown); break;
    // TODO: medicine cat icon
    }
  }

  public override bool CanDrop (Phase phase, object dragState) =>
    phase == Phase.Eat && dragState is Prey;

  protected override object DropState => cat;

  private void ShowTypeIcon (Sprite icon) {
    type.sprite = icon;
    type.gameObject.SetActive(true);
  }
}

}
