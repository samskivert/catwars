namespace catwars {

using UnityEngine;
using TMPro;

using React;

public class ClanController : MonoBehaviour {
  private GameState game;
  private ClanState clan;

  public TMP_Text clanLabel;
  public GameObject cats;
  public GameObject catPrefab;
  public HerbController[] herbs;
  public FoodController food;
  public MessagesController messages;

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
    var idx = 0; foreach (var herbctl in herbs) {
      var herb = (Herb)idx++;
      herbctl.SetHerb(herb);
      clan.herbs.GetValue(herb).OnValue(herbctl.SetCount);
    }
    clan.freshKill.OnValue(food.Show);

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
