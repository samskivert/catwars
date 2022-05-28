namespace catwars {

using UnityEngine;
using TMPro;

public class ClanController : MonoBehaviour {
  private GameState game;
  private ClanState clan;

  public TMP_Text clanLabel;
  public GameObject cats;
  public GameObject catPrefab;

  public void Init (GameState game, ClanState clan) {
    this.game = game;
    this.clan = clan;
    clanLabel.text = clan.name;
    foreach (var cat in clan.cats) AddCat(cat);
  }

  private void AddCat (CatState cat) {
    var catObj = Instantiate(catPrefab, cats.transform);
    catObj.GetComponent<CatController>().Init(cat);
  }
}

}
