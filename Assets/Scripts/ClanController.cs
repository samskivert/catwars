namespace catwars {

using System.Collections.Generic;

using UnityEngine;
using TMPro;

public class ClanController : MonoBehaviour {
  private GameState game;
  private ClanState clan;

  public TMP_Text clanLabel;
  public GameObject cats;
  public GameObject catPrefab;
  public HerbController[] herbs;

  public void Init (GameState game, ClanState clan) {
    this.game = game;
    this.clan = clan;
    clanLabel.text = clan.name;
    foreach (var cat in clan.cats) AddCat(cat);
    for (var ii = 0; ii < herbs.Length; ii += 1) herbs[ii].Show(ii, clan.herbs.GetValueOrDefault(ii));
  }

  private void AddCat (CatState cat) {
    var catObj = Instantiate(catPrefab, cats.transform);
    catObj.GetComponent<CatController>().Init(cat);
    catObj.SetActive(true);
  }
}

}
