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
  }

  private void AddCat (CatState cat) {
    var catObj = Instantiate(catPrefab, cats.transform);
    catObj.GetComponent<CatController>().Init(cat);
    catObj.SetActive(true);
  }
}

}
