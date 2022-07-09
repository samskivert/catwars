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

  public readonly IMutable<CatState> draggedCat = Values.Mutable<CatState>(null);
  public readonly IMutable<PlaceController> hoveredPlace = Values.Mutable<PlaceController>(null);
  public readonly IValue<(CatState, PlaceController)> dragInfo;

  private ClanController () {
    dragInfo = Values.Join(draggedCat, hoveredPlace);
  }

  public void Init (GameState game, ClanState clan) {
    this.game = game;
    this.clan = clan;
    clanLabel.text = clan.name;
    foreach (var cat in clan.cats) AddCat(cat);
    var idx = 0; foreach (var herb in herbs) {
      herb.SetHerb(idx);
      clan.herbs.GetValue(idx++).OnValue(herb.SetCount);
    }
    clan.freshKill.OnValue(food.Show);

    dragInfo.OnChange((info, oinfo) => {
      var (cat, place) = info;
      var (ocat, oplace) = oinfo;
      // if we transition from cat to no cat while hovering over a place, we dropped a cat there
      if (oplace != null && place == oplace &&
          cat == null && ocat != null) OnCatDropped(oinfo.Item1, info.Item2);
    });
  }

  private void AddCat (CatState cat) {
    var catObj = Instantiate(catPrefab, cats.transform);
    catObj.GetComponent<CatController>().Init(cat);
    catObj.SetActive(true);
  }

  private void OnCatDropped (CatState cat, PlaceController place) {
    Debug.Log("Cat dropped " + cat + " => " + place);
  }
}

}
