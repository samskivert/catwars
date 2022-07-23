namespace catwars {

using UnityEngine;
using TMPro;

using React;

public class ClanController : MonoBehaviour {
  private GameState game;
  private ClanState clan;

  public MessagesController messages;
  public TMP_Text clanLabel;
  public GameObject cats;
  public GameObject catPrefab;
  public HerbController[] herbs;
  public GameObject freshKill;
  public GameObject preyPrefab;

  public readonly IMutable<object> draggedItem = Values.Mutable<object>(null);
  public readonly IMutable<object> hoveredTarget = Values.Mutable<object>(null);
  public readonly IValue<(object, object)> dragInfo;

  private ClanController () {
    dragInfo = Values.Join(draggedItem, hoveredTarget);
  }

  public void Init (GameState game, ClanState clan) {
    this.game = game;
    this.clan = clan;
    clanLabel.text = clan.name;
    foreach (var cat in clan.cats) AddCat(cat);

    var hidx = 0; foreach (var herbctl in herbs) {
      var herb = (Herb)hidx++;
      herbctl.SetHerb(herb);
      clan.herbs.GetValue(herb).OnValue(herbctl.SetCount);
    }

    clan.freshKill.OnEntries((idx, prey, oprey) => {
      var tx = freshKill.transform;
      if (tx.childCount < idx) tx.GetChild(idx).GetComponent<PreyController>().SetPrey(prey);
      else Instantiate(preyPrefab, tx).GetComponent<PreyController>().SetPrey(prey);
    });
    clan.freshKill.OnRemove((idx, oprey) => {
      Destroy(freshKill.transform.GetChild(idx));
    });

    dragInfo.OnChange((info, oinfo) => {
      var (item, tgt) = info;
      var (oitem, otgt) = oinfo;
      // if we transition from cat to no cat while hovering over a place, we dropped a cat there
      if (tgt is Place place && tgt == otgt &&
          item == null && oitem is CatState cat) OnCatDropped(cat, place);
    });

    clan.messages.OnEmit(messages.Show);
  }

  private void AddCat (CatState cat) {
    var catObj = Instantiate(catPrefab, cats.transform);
    catObj.GetComponent<CatController>().Init(cat);
    catObj.SetActive(true);
  }

  private void OnCatDropped (CatState cat, Place place) {
    cat.OnDrop(place);
  }
}

}
